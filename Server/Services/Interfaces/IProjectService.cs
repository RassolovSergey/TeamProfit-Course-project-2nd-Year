using Server.DTO.Product;
using Server.DTO.Project;

namespace Server.Services.Interfaces
{
    public interface IProjectService
    {
                    // Мытоды работы со СПИСКАМИ

        /// <summary>Получить список всех проектов пользователя</summary>
        Task<List<ProjectDto>> GetByUserAsync(int userId);


                    // Мытоды работы СУЩНОСТЬЮ

        /// <summary>Получить проект по идентификатору или null, если не найден</summary>
        Task<ProjectDto?> GetByIdAsync(int id);

        /// <summary>Создать новый проект и вернуть его DTO</summary>
        Task<ProjectDto> CreateAsync(CreateProjectDto dto, int creatorUserId);

        /// <summary>
        /// Обновить существующий проект,
        /// вернуть обновленный DTO или null, если не найден
        /// </summary>
        Task<ProjectDto?> UpdateAsync(int id, UpdateProjectDto dto);

        /// <summary>Удалить проект по идентификатору, вернуть true при успехе</summary>
        Task<bool> DeleteAsync(int id);
    }
}
