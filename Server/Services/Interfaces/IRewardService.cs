// Server/Services/Interfaces/IRewardService.cs
using Server.DTO.Reward;
using Server.DTO.Product;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Server.Services.Interfaces
{
    /// <summary>Сервис бизнес-логики для наград</summary>
    public interface IRewardService
    {
        Task<List<RewardDto>> GetAllAsync();
        Task<List<RewardDto>> GetByProjectAsync(int projectId);
        Task<RewardDto?> GetByIdAsync(int id);

        Task<RewardDto> CreateAsync(CreateRewardDto dto, int projectId);
        Task<RewardDto?> UpdateAsync(int id, UpdateRewardDto dto);
        Task<bool> DeleteAsync(int id);

        // Новые методы:
        /// <summary>Привязать продукт к награде</summary>
        Task<bool> AddProductAsync(int rewardId, int productId);

        /// <summary>Открепить продукт от награды</summary>
        Task<bool> RemoveProductAsync(int rewardId, int productId);

        /// <summary>Получить все продукты, связанные с наградой</summary>
        Task<List<ProductDto>> GetProductsByRewardAsync(int rewardId);
    }
}
