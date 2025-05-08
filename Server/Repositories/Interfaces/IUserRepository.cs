using Data.Entities;
using Server.Repositories.Interfaces.Generic_Repository;

namespace Server.Repositories.Interfaces
{
    /// <summary>
    /// CRUD-операции для сущности User
    /// </summary>
    public interface IUserRepository : IGenericRepository<User>
    {

    }
}
