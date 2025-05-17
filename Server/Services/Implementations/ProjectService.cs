using AutoMapper;
using Data.Context;
using Data.Entities;
using Data.Enums;
using Microsoft.EntityFrameworkCore;
using Server.DTO.Project;
using Server.Repositories.Interfaces.Generic_Repository;
using Server.Services.Interfaces;

namespace Server.Services.Implementations
{
    public class ProjectService : IProjectService
    {
        private readonly IGenericRepository<Project> _projRepo;
        private readonly IGenericRepository<UserProject> _upRepo;
        private readonly AppDbContext _db;
        private readonly IMapper _mapper;

        public ProjectService(
            IGenericRepository<Project> projRepo,
            IGenericRepository<UserProject> upRepo,
            AppDbContext db,
            IMapper mapper)
        {
            _projRepo = projRepo;
            _upRepo = upRepo;
            _db = db;
            _mapper = mapper;
        }

        public async Task<List<ProjectDto>> GetAllAsync()
        {
            var projects = await _projRepo.GetAllAsync();
            return _mapper.Map<List<ProjectDto>>(projects);
        }

        public async Task<ProjectDto?> GetByIdAsync(int id)
        {
            var project = await _projRepo.GetByIdAsync(id);
            return project is null
                ? null
                : _mapper.Map<ProjectDto>(project);
        }

        public async Task<ProjectDto> CreateAsync(CreateProjectDto dto)
        {
            // 1) сохраняем проект
            var project = _mapper.Map<Project>(dto);
            project.DateClose = dto.DateStart.AddDays(dto.ProjectDuration);
            await _projRepo.AddAsync(project);
            await _projRepo.SaveChangesAsync();

            // 2) готовим запись администратора
            var adminUp = new UserProject
            {
                ProjectId = project.Id,
                UserId = dto.CreatorUserId,
                TypeCooperation = TypeCooperation.FixedPayment,
                FixedPrice = 0m,
                PercentPrice = 0m,
                IsAdmin = true
            };

            // **Важное добавление: прикрепляем «заглушку» пользователя в контекст**  
            // чтобы EF Core не пытался вставить нового User
            _db.Entry(new User { Id = dto.CreatorUserId })
               .State = EntityState.Unchanged;

            await _upRepo.AddAsync(adminUp);
            await _upRepo.SaveChangesAsync();

            // 3) подгружаем участников для DTO
            await _db.Entry(project)
                     .Collection(p => p.UserProjects)
                     .LoadAsync();

            return _mapper.Map<ProjectDto>(project);
        }


        public async Task<ProjectDto?> UpdateAsync(int id, UpdateProjectDto dto)
        {
            var project = await _projRepo.GetByIdAsync(id);
            if (project is null) return null;

            // Обновляем поля
            project.Name = dto.Name;
            project.Description = dto.Description;
            project.DateStart = dto.DateStart;
            project.ProjectDuration = dto.ProjectDuration;
            project.DateClose = dto.DateStart.AddDays(dto.ProjectDuration);

            await _projRepo.UpdateAsync(project);
            await _projRepo.SaveChangesAsync();

            // Обновляем навигацию участников для DTO
            await _db.Entry(project)
                     .Collection(p => p.UserProjects)
                     .LoadAsync();

            return _mapper.Map<ProjectDto>(project);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var project = await _projRepo.GetByIdAsync(id);
            if (project is null) return false;

            // Сначала удаляем всех участников (cascade должен сработать, но на всякий случай)
            var ups = (await _upRepo.GetAllAsync())
                        .Where(up => up.ProjectId == id)
                        .ToList();
            _db.Set<UserProject>().RemoveRange(ups);
            await _db.SaveChangesAsync();

            // Удаляем сам проект
            await _projRepo.DeleteAsync(id);
            await _projRepo.SaveChangesAsync();
            return true;
        }
    }
}
