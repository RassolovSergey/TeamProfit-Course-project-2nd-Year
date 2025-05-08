using Data.Context;
using Data.Entities;
using Server.Repositories.Implementations.GenericRepository;
using Server.Repositories.Interfaces;

namespace Server.Repositories.Implementations
{
    public class CurrencyRepository : GenericRepository<Currency>, ICurrencyRepository
    {
        // Вызываем конструктор базового класса
        public CurrencyRepository(AppDbContext db) : base(db)
        {
        }
    }
}
