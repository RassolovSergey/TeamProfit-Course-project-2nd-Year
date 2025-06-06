// Server/Services/Implementations/UserProjectService.cs
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using AutoMapper;
using Data.Context;
using Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.DTO.UserProject;
using Server.Repositories.Interfaces;
using Server.Repositories.Interfaces.Generic_Repository;
using Server.Services.Interfaces;

namespace Server.Services.Implementations
{
    public class UserProjectService : IUserProjectService
    {
        private readonly IGenericRepository<UserProject> _repo;
        private readonly IMapper _mapper;
        private readonly AppDbContext _db;
        private readonly IUserProjectRepository _upRepo;

        public UserProjectService(
            IGenericRepository<UserProject> repo,
            AppDbContext db,
            IUserProjectRepository upRepo,
            IMapper mapper)
        {
            _repo = repo;
            _db = db;       // <- сохраняем контекст
            _mapper = mapper;
            _upRepo = upRepo;
        }

        public async Task<List<UserProjectDto>> GetAllAsync()
        {
            var all = await _repo.GetAllAsync();
            return _mapper.Map<List<UserProjectDto>>(all);
        }


        public async Task<UserProjectDto?> GetAsync(int userId, int projectId)
        {
            var all = await _repo.GetAllAsync();
            var e = all.FirstOrDefault(up => up.UserId == userId && up.ProjectId == projectId);
            return e is null ? null : _mapper.Map<UserProjectDto>(e);
        }

        public async Task<List<UserProjectDto>> GetByProjectAsync(int projectId)
        {
            var all = await _repo.GetAllAsync();
            var filtered = all.Where(up => up.ProjectId == projectId).ToList();
            return _mapper.Map<List<UserProjectDto>>(filtered);
        }

        // UserProjectService.cs
        public async Task<UserProjectDto> CreateAsync(int projectId, CreateUserProjectDto dto)
        {
            var up = new UserProject
            {
                ProjectId = projectId,
                UserId = dto.UserId,
                TypeCooperation = dto.TypeCooperation,
                FixedPrice = dto.FixedPrice,
                PercentPrice = dto.PercentPrice,
                // всегда по умолчанию: новый участник — не админ
                IsAdmin = false
            };

            await _repo.AddAsync(up);
            await _repo.SaveChangesAsync();

            return _mapper.Map<UserProjectDto>(up);
        }


        public async Task<UserProjectDto?> UpdateAsync(int userId, int projectId, UpdateUserProjectDto dto)
        {
            var all = await _repo.GetAllAsync();
            var e = all.FirstOrDefault(up => up.UserId == userId && up.ProjectId == projectId);
            if (e is null) return null;
            _mapper.Map(dto, e);
            e.IsAdmin = false;
            await _repo.UpdateAsync(e);
            await _repo.SaveChangesAsync();
            return _mapper.Map<UserProjectDto>(e);
        }

        public async Task<bool> DeleteAsync(int userId, int projectId)
        {
            var all = await _repo.GetAllAsync();
            var e = all.FirstOrDefault(up => up.UserId == userId && up.ProjectId == projectId);
            if (e is null)
                return false;

            // Удаляем напрямую через DbContext
            _db.Set<UserProject>().Remove(e);
            await _db.SaveChangesAsync();
            return true;
        }

        public Task<bool> IsMemberAsync(int projectId, int userId)
            => _upRepo.AnyAsync(up => up.ProjectId == projectId && up.UserId == userId);

        public Task<bool> IsAdminAsync(int projectId, int userId)
            => _upRepo.AnyAsync(up => up.ProjectId == projectId && up.UserId == userId && up.IsAdmin);


        public async Task<bool> AnyAsync(Expression<Func<UserProject, bool>> predicate) => await _upRepo.AnyAsync(predicate);

        public async Task<List<UserProjectDto>> GetByUserAsync(int userId)
        {
            var userProjects = await _db.UserProjects
                                        .Where(up => up.UserId == userId)
                                        .ToListAsync();

            return _mapper.Map<List<UserProjectDto>>(userProjects);
        }

    }
}
