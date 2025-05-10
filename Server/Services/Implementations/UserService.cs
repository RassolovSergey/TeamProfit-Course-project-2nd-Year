using AutoMapper;
using Data.Entities;
using System.Security.Cryptography;
using System.Text;
using Server.DTO.User;
using Server.Repositories.Interfaces;
using Server.Services.Interfaces;

namespace Server.Services.Implementations
{
    // Реализация бизнес-логики для работы с пользователями
    public class UserService : IUserService
    {
        // Ссылка на репозиторий для операций с базой данных по сущности User
        private readonly IUserRepository _repo;
        // Ссылка на AutoMapper для маппинга между Entity и DTO
        private readonly IMapper _mapper;

        // Конструктор, принимающий зависимости через DI
        public UserService(IUserRepository repo, IMapper mapper)
        {
            _repo = repo;       // Инициализируем репозиторий
            _mapper = mapper;   // Инициализируем маппер
        }

        // Метод для получения списка всех пользователей
        public async Task<List<UserDto>> GetAllAsync()
        {
            var users = await _repo.GetAllAsync();      // Получаем все User из БД
            return _mapper.Map<List<UserDto>>(users);   // Конвертируем список Entity в список DTO
        }

        // Метод для получения пользователя по ID
        public async Task<UserDto?> GetByIdAsync(int id)
        {
            var user = await _repo.GetByIdAsync(id);    // Ищем User по первичному ключу
            return user is null 
                ? null                                  // Если не найден — возвращаем null
                : _mapper.Map<UserDto>(user);           // Иначе маппим Entity в DTO
        }

        // Метод для создания нового пользователя
        public async Task<UserDto> CreateAsync(CreateUserDto dto)
        {
            // Конвертируем CreateUserDto в Entity User (без пароля)
            var user = _mapper.Map<User>(dto);

            // Генерация соли и хэша
            user.PasswordSalt = Convert.ToBase64String
                (RandomNumberGenerator.GetBytes(16)); // Создаем 16 байт случайной соли и кодируем в Base64
            
            using var sha = SHA256.Create();    // Инициализируем SHA-256 для хеширования
            var hashed = sha.ComputeHash
                (Encoding.UTF8.GetBytes(dto.Password + user.PasswordSalt)); // Получаем байты от строки "пароль+соль"
            user.HashPassword = Convert.ToBase64String(hashed);             // Кодируем хеш в Base64 и сохраняем

            await _repo.AddAsync(user);         // Буферизуем добавление новой записи в БД
            await _repo.SaveChangesAsync();     // Физически сохраняем изменения

            return _mapper.Map<UserDto>(user);   // Возвращаем созданного пользователя в виде DTO
        }

        // Метод для удаления пользователя по ID
        public async Task<bool> DeleteAsync(int id)
        {
            var exists = await _repo.GetByIdAsync(id) != null;  // Проверяем, существует ли пользователь
            if (!exists) return false;      // Если нет — возвращаем false

            await _repo.DeleteAsync(id);    // Буферизуем удаление записи
            await _repo.SaveChangesAsync(); // Сохраняем изменения
            return true;                    // Возвращаем успешный результат
        }

        // Метод для обновления данных пользователя
        public async Task<UserDto?> UpdateAsync(int id, UpdateUserDto dto)
        {
            // 1. Получаем существующего пользователя
            var user = await _repo.GetByIdAsync(id);
            if (user is null)   
                return null;        // Если не найден — возвращаем null

            // 2. Накатываем изменения из DTO на существующую сущность
            _mapper.Map(dto, user);

            // 3. Сохраняем изменения
            await _repo.UpdateAsync(user);  // Буферизуем изменения в сущности
            await _repo.SaveChangesAsync(); // Сохраняем изменения в БД

            // 4. Возвращаем обновлённый DTO
            return _mapper.Map<UserDto>(user);   // Конвертируем обновленную сущность в DTO и возвращаем
        }
    }
}
