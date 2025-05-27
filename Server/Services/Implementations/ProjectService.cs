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

        // Конструктор
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

                        // Методы работы со СПИСКОМ СУЩНОСТЕЙ

        public async Task<List<ProjectDto>> GetByUserAsync(int userId)
        {
            // Предполагаем, что есть связь UserProject → Project
            var projects = await _db.UserProjects
                                    .Where(up => up.UserId == userId)
                                    .Select(up => up.Project)
                                    .ToListAsync();
            return _mapper.Map<List<ProjectDto>>(projects);
        }


                        // Методы работы с СУЩНОСТЬЮ
        // ПОЛУУЧИТЬ ПО ID
        public async Task<ProjectDto?> GetByIdAsync(int id)
        {
            var project = await _projRepo.GetByIdAsync(id);
            return project is null
                ? null
                : _mapper.Map<ProjectDto>(project);
        }

        // СОЗДАТЬ
        public async Task<ProjectDto> CreateAsync(CreateProjectDto dto, int creatorUserId)
        {
            // 1) создаём и сохраняем проект
            var project = _mapper.Map<Project>(dto);
            project.DateClose = project.DateStart.AddDays(project.ProjectDuration);

            var now = DateTime.UtcNow;
            project.Status = now < project.DateStart
                ? ProjectStatus.PlannedProject
                : now <= project.DateClose
                    ? ProjectStatus.CurrentProject
                    : ProjectStatus.CompletedProject;

            await _projRepo.AddAsync(project);
            await _projRepo.SaveChangesAsync();

            // 2) привязываем администратора проекта (тот, кто создаёт)
            var adminLink = new UserProject
            {
                ProjectId = project.Id,
                UserId = creatorUserId,
                TypeCooperation = TypeCooperation.FixedPayment,
                FixedPrice = 0m,
                PercentPrice = 0m,
                IsAdmin = true
            };
            // чтобы EF не пытался создать нового пользователя
            _db.Entry(new User { Id = creatorUserId }).State = EntityState.Unchanged;

            await _upRepo.AddAsync(adminLink);
            await _upRepo.SaveChangesAsync();

            // 3) подгружаем участников и возвращаем DTO
            await _db.Entry(project)
                     .Collection(p => p.UserProjects)
                     .LoadAsync();

            return _mapper.Map<ProjectDto>(project);
        }



        // ОБНОВИТЬ по Id
        public async Task<ProjectDto?> UpdateAsync(int id, UpdateProjectDto dto)
        {
            // 1) Найти сущность
            var project = await _projRepo.GetByIdAsync(id);
            if (project is null)
                return null;

            // 2) Обновить только те поля, что пришли
            if (!string.IsNullOrEmpty(dto.Name))
                project.Name = dto.Name;

            if (dto.Description != null)
                project.Description = dto.Description;

            if (dto.DateStart.HasValue)
                project.DateStart = dto.DateStart.Value;

            if (dto.ProjectDuration.HasValue)
            {
                project.ProjectDuration = dto.ProjectDuration.Value;
                // если длительность поменялась — пересчитать DateClose
                project.DateClose = project.DateStart.AddDays(project.ProjectDuration);
            }

            if (dto.CurrencyId.HasValue)
                project.CurrencyId = dto.CurrencyId.Value;

            // 3) Обновить статус (если дата/duration изменились)
            var now = DateTime.UtcNow;
            project.Status = now < project.DateStart
                ? ProjectStatus.PlannedProject
                : now <= project.DateClose
                    ? ProjectStatus.CurrentProject
                    : ProjectStatus.CompletedProject;

            // 4) Сохранить
            await _projRepo.SaveChangesAsync();

            // 5) Загрузить участников, если нужна коллекция в DTO
            // await _db.Entry(project).Collection(p => p.UserProjects).LoadAsync();

            return _mapper.Map<ProjectDto>(project);
        }


        // УДАЛИТЬ ПО Id
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
