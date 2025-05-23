using Data.Context;
using Data.Entities;
using Server.Repositories.Implementations.GenericRepository;
using Server.Repositories.Interfaces;

namespace Server.Repositories.Implementations
{
    /// <summary>
    /// Реализация репозитория для сущности Currency на EF Core
    /// </summary>
    public class CurrencyRepository : GenericRepository<Currency>, ICurrencyRepository
    {
        public CurrencyRepository(AppDbContext db)
            : base(db)
        {
        }
    }
}
