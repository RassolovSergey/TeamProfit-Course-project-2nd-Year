using Data.Entities;
using Server.Repositories.Interfaces.Generic_Repository;

namespace Server.Repositories.Interfaces
{
    /// <summary>
    /// CRUD-операции для сущности Sale
    /// </summary>
    public interface ISaleRepository : IGenericRepository<Sale>
    {
        Task<List<Sale>> GetAllWithRewardsAsync();
    }
}
