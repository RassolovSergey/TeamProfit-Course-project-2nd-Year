// Server/Repositories/Implementations/ProductRepository.cs
using Data.Context;
using Data.Entities;
using Microsoft.EntityFrameworkCore;
using Server.Repositories.Implementations.GenericRepository;
using Server.Repositories.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Repositories.Implementations
{
    /// <summary>EF-Core реализация репозитория продуктов</summary>
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        private readonly AppDbContext _db;

        public ProductRepository(AppDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task<List<Product>> GetByRewardIdAsync(int rewardId)
        {
            return await _db.Products
                            .Where(p => p.Rewards.Any(r => r.Id == rewardId))
                            .ToListAsync();
        }
    }
}
