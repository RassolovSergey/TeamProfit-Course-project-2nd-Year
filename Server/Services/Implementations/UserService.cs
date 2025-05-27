using AutoMapper;
using Data.Context;
using Data.Entities;
using Microsoft.EntityFrameworkCore;
using Server.DTO.User;
using Server.Repositories.Interfaces;
using Server.Services.Interfaces;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Server.Services.Implementations
{
    // Реализация бизнес-логики для работы с пользователями
    public class UserService : IUserService
    {
        private readonly IUserRepository _repo;
        private readonly IProjectService _projectService;
        private readonly AppDbContext _db;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;

        public UserService(
            IUserRepository repo,
            IProjectService projectService,
            AppDbContext db,
            IMapper mapper,
            IConfiguration config)
        {
            _repo = repo;
            _projectService = projectService;
            _db = db;
            _mapper = mapper;
            _config = config;
        }

        public async Task<List<UserDto>> GetAllAsync()
        {
            var users = await _repo.GetAllAsync();
            return _mapper.Map<List<UserDto>>(users);
        }

        public async Task<UserDto?> GetByIdAsync(int id)
        {
            var user = await _repo.GetByIdAsync(id);
            return user is null
                ? null
                : _mapper.Map<UserDto>(user);
        }

        public async Task<UserDto> CreateAsync(CreateUserDto dto)
        {
            var user = _mapper.Map<User>(dto);

            // генерируем соль и хеш
            user.PasswordSalt = Convert.ToBase64String(RandomNumberGenerator.GetBytes(16));
            using var sha = SHA256.Create();
            var hashed = sha.ComputeHash(Encoding.UTF8.GetBytes(dto.Password + user.PasswordSalt));
            user.HashPassword = Convert.ToBase64String(hashed);

            await _repo.AddAsync(user);
            await _repo.SaveChangesAsync();

            return _mapper.Map<UserDto>(user);
        }

        /// <summary>
        /// При удалении пользователя:
        /// 1) удаляем все проекты, где он был администратором,
        /// 2) чистим оставшиеся ссылки UserProject,
        /// 3) удаляем самого пользователя.
        /// </summary>
        public async Task<bool> DeleteAsync(int id)
        {
            var user = await _repo.GetByIdAsync(id);
            if (user is null)
                return false;

            // 1) Список проектов, где этот пользователь – админ
            var adminProjectIds = await _db.UserProjects
                .Where(up => up.UserId == id && up.IsAdmin)
                .Select(up => up.ProjectId)
                .ToListAsync();

            // 2) Удаляем каждый проект через сервис проектов
            foreach (var projectId in adminProjectIds)
            {
                await _projectService.DeleteAsync(projectId);
            }

            // 3) Чистим любые оставшиеся ссылки этого пользователя
            var leftovers = await _db.UserProjects
                .Where(up => up.UserId == id)
                .ToListAsync();
            if (leftovers.Any())
            {
                _db.UserProjects.RemoveRange(leftovers);
                await _db.SaveChangesAsync();
            }

            // 4) Удаляем самого пользователя
            await _repo.DeleteAsync(id);
            await _repo.SaveChangesAsync();

            return true;
        }

        // Server/Services/Implementations/UserService.cs
        public async Task<UserDto?> UpdateAsync(int id, UpdateUserDto dto)
        {
            var user = await _repo.GetByIdAsync(id);
            if (user is null) return null;

            // 1) Обновляем простые поля
            if (!string.IsNullOrEmpty(dto.Login)) user.Login = dto.Login;
            if (!string.IsNullOrEmpty(dto.Email)) user.Email = dto.Email;
            if (!string.IsNullOrEmpty(dto.Name)) user.Name = dto.Name;
            if (!string.IsNullOrEmpty(dto.SurName)) user.SurName = dto.SurName;
            if (dto.CurrencyId.HasValue) user.CurrencyId = dto.CurrencyId.Value;

            // 2) Если пользователь хочет сменить пароль
            if (!string.IsNullOrEmpty(dto.NewPassword))
            {
                // 2.1) Проверяем, что CurrentPassword соответствует тому, что в базе
                using var sha = SHA256.Create();
                var currentHash = Convert.ToBase64String(
                    sha.ComputeHash(Encoding.UTF8.GetBytes(dto.CurrentPassword! + user.PasswordSalt))
                );
                if (currentHash != user.HashPassword)
                    throw new ArgumentException("Текущий пароль указан неверно.");

                // 2.2) Генерируем новую соль и хеш для NewPassword
                SetNewPassword(user, dto.NewPassword!);
            }

            // 3) Сохраняем изменения
            await _repo.SaveChangesAsync();
            return _mapper.Map<UserDto>(user);
        }


        /// <summary>Проверяет, что plainPassword при хешировании совпадёт с user.HashPassword</summary>
        private bool VerifyPassword(User user, string plainPassword)
        {
            using var sha = SHA256.Create();
            var computed = sha.ComputeHash(Encoding.UTF8.GetBytes(plainPassword + user.PasswordSalt));
            return Convert.ToBase64String(computed) == user.HashPassword;
        }


        /// <summary>
        /// Генерирует соль и хеширует пароль через SHA-256 (пароль + соль).
        /// </summary>
        private void SetNewPassword(User user, string plainPassword)
        {
            // 1) соль — 16 случайных байт
            var saltBytes = RandomNumberGenerator.GetBytes(16);
            var salt = Convert.ToBase64String(saltBytes);

            // 2) SHA-256(password + salt)
            using var sha = SHA256.Create();
            var hashBytes = sha.ComputeHash(Encoding.UTF8.GetBytes(plainPassword + salt));
            var hash = Convert.ToBase64String(hashBytes);

            // 3) сохраняем
            user.PasswordSalt = salt;
            user.HashPassword = hash;
        }


        public async Task<string?> AuthenticateAsync(string login, string password)
        {
            var user = (await _repo.GetAllAsync())
                .FirstOrDefault(u => u.Login == login);
            if (user is null) return null;

            using var sha = SHA256.Create();
            var computed = sha.ComputeHash(
                Encoding.UTF8.GetBytes(password + user.PasswordSalt));
            if (Convert.ToBase64String(computed) != user.HashPassword)
                return null;

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub,         user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.Login),
                new Claim(ClaimTypes.Email,                   user.Email)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var expireMinutes = int.Parse(_config["Jwt:ExpireMinutes"] ?? "60");
            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expireMinutes),
                signingCredentials: creds);


            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
