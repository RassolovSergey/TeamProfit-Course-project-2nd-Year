// Server/Services/Implementations/UserProjectService.cs
using System.Linq;
using AutoMapper;
using Data.Context;
using Data.Entities;
using Server.DTO.UserProject;
using Server.Repositories.Interfaces.Generic_Repository;
using Server.Services.Interfaces;

namespace Server.Services.Implementations
{
    public class UserProjectService : IUserProjectService
    {
        private readonly IGenericRepository<UserProject> _repo;
        private readonly IMapper _mapper;
        private readonly AppDbContext _db;

        public UserProjectService(
            IGenericRepository<UserProject> repo,
            AppDbContext db,
            IMapper mapper)
        {
            _repo = repo;
            _db = db;       // <- сохраняем контекст
            _mapper = mapper;
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

        public async Task<UserProjectDto> CreateAsync(CreateUserProjectDto dto)
        {
            var e = _mapper.Map<UserProject>(dto);
            e.IsAdmin = false;
            await _repo.AddAsync(e);
            await _repo.SaveChangesAsync();
            return _mapper.Map<UserProjectDto>(e);
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
    }
}
