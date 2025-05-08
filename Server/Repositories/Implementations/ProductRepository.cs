using Data.Context;
using Data.Entities;
using Server.Repositories.Implementations.GenericRepository;
using Server.Repositories.Interfaces;

namespace Server.Repositories.Implementations
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        // Вызываем конструктор базового класса
        public ProductRepository(AppDbContext db) : base(db)
        {
        }
    }
}
