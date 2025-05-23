using Data.Context;
using Data.Entities;
using Microsoft.EntityFrameworkCore;
using Server.Repositories.Implementations.GenericRepository;
using Server.Repositories.Interfaces;

namespace Server.Repositories.Implementations
{
    /// <summary>Реализация репозитория для продаж</summary>
    public class SaleRepository : GenericRepository<Sale>, ISaleRepository
    {
        private readonly AppDbContext _db;

        public SaleRepository(AppDbContext db) : base(db)
        {
            _db = db;
        }

        /// <summary>
        /// Возвращает все продажи с подгруженными Reward
        /// </summary>
        public async Task<List<Sale>> GetAllWithRewardsAsync()
        {
            return await _db.Sales.Include(s => s.Reward).ToListAsync();
        }
    }
}
