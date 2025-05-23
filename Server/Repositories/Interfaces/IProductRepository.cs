// Server/Repositories/Interfaces/IProductRepository.cs
using Data.Entities;
using Server.Repositories.Interfaces.Generic_Repository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Server.Repositories.Interfaces
{
    /// <summary>Репозиторий для сущности Product</summary>
    public interface IProductRepository : IGenericRepository<Product>
    {
        /// <summary>Получить все продукты, связанные с данной наградой</summary>
        Task<List<Product>> GetByRewardIdAsync(int rewardId);
    }
}
