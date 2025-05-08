using Data.Context;
using Data.Entities;
using Server.Repositories.Implementations.GenericRepository;
using Server.Repositories.Interfaces;

namespace Server.Repositories.Implementations
{
    public class CostRepository : GenericRepository<Cost>, ICostRepository
    {
        // Вызываем конструктор базового класса
        public CostRepository(AppDbContext db) : base(db)
        {
        }
    }
}
 