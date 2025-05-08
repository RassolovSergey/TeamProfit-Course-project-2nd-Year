using Data.Context;
using Data.Entities;
using Server.Repositories.Implementations.GenericRepository;
using Server.Repositories.Interfaces;

namespace Server.Repositories.Implementations
{
    public class SaleRepository : GenericRepository<Sale>, ISaleRepository
    {
        // Вызываем конструктор базового класса
        public SaleRepository(AppDbContext db) : base(db)
        {
        }
    }
}
