using System.Linq.Expressions;
using Data.Entities;
using Server.DTO.UserProject;

namespace Server.Services.Interfaces
{
    public interface IUserProjectService
    {
        // Проверяет, что пользователь состоит в проекте (и может его просматривать)
        Task<bool> IsMemberAsync(int projectId, int userId);

        // Проверяет, что пользователь администратор проекта
        Task<bool> IsAdminAsync(int projectId, int userId);

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

        /// <summary>
        /// Проверяет, существует ли запись UserProject по заданному условию.
        /// </summary>
        Task<bool> AnyAsync(Expression<Func<UserProject, bool>> predicate);
    }

}
