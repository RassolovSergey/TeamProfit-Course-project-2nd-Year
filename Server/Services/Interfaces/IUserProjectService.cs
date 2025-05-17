using Data.Entities;
using Server.DTO.UserProject;

namespace Server.Services.Interfaces
{
    public interface IUserProjectService
    {
        // Получить все данные из таблицы
        Task<List<UserProjectDto>> GetAllAsync();

        // Получить по составному ключу
        Task<UserProjectDto?> GetAsync(int userId, int projectId);

        // Создать запись
        Task<UserProjectDto> CreateAsync(CreateUserProjectDto dto);

        // Обновить по составному ключу
        Task<UserProjectDto?> UpdateAsync(int userId, int projectId, UpdateUserProjectDto dto);

        // Удалить по составному ключу
        Task<bool> DeleteAsync(int userId, int projectId);

        // Удаление по сущности
        Task<List<UserProjectDto>> GetByProjectAsync(int projectId);
    }
}
