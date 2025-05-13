using Data.Entities;
using Server.Repositories.Interfaces.Generic_Repository;

namespace Server.Repositories.Interfaces
{
    /// <summary>
    /// CRUD-операции для сущности UserProject
    /// </summary>
    public interface IUserProjectRepository : IGenericRepository<UserProject>
    {
        Task<UserProject?> GetByIdsAsync(int userId, int projectId);
        Task DeleteByIdsAsync(int userId, int projectId);
    }
}
