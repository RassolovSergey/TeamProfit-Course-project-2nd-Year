using Data.Entities;
using Server.Repositories.Interfaces.Generic_Repository;

namespace Server.Repositories.Interfaces
{
    /// <summary>
    /// Интерфейс репозитория для работы с сущностью Currency
    /// </summary>
    public interface ICurrencyRepository : IGenericRepository<Currency>
    {
        
    }
}
