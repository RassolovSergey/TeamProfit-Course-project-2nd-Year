using Data.Entities;
using Server.Repositories.Interfaces.Generic_Repository;

namespace Server.Repositories.Interfaces
{
    /// <summary>
    /// CRUD-операции для сущности Team
    /// </summary>
    public interface ITeamRepository : IGenericRepository<Team>
    {
        
    }
}
