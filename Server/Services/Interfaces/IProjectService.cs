using Server.DTO.Project;

namespace Server.Services.Interfaces
{
    public interface IProjectService
    {
        /// <summary>Получить список всех проектов</summary>
        Task<List<ProjectDto>> GetAllAsync();

        /// <summary>Получить проект по идентификатору или null, если не найден</summary>
        Task<ProjectDto?> GetByIdAsync(int id);

        /// <summary>Создать новый проект и вернуть его DTO</summary>
        Task<ProjectDto> CreateAsync(CreateProjectDto dto);

        /// <summary>Обновить существующий проект, вернуть обновлённый DTO или null, если не найдено</summary>
        Task<ProjectDto?> UpdateAsync(int id, UpdateProjectDto dto);

        /// <summary>Удалить проект по идентификатору, вернуть true, если успешно</summary>
        Task<bool> DeleteAsync(int id);
    }
}
