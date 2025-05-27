using Server.DTO.Product;
using Server.DTO.Reward;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Server.Services.Interfaces
{
    public interface IRewardService
    {
        /// <summary>
        /// Получить список всех наград проекта.
        /// Пользователь должен быть участником или администратором проекта.
        /// </summary>
        Task<List<RewardDto>> GetByProjectAsync(int projectId, int userId);

        /// <summary>
        /// Создать новую награду в проекте.
        /// Только администратор проекта.
        /// </summary>
        Task<RewardDto> CreateRewardAsync(int projectId, CreateRewardDto dto, int userId);

        /// <summary>
        /// Обновить награду по Id.
        /// Только администратор проекта.
        /// </summary>
        Task<RewardDto?> UpdateRewardAsync(int rewardId, UpdateRewardDto dto, int userId);

        /// <summary>
        /// Удалить награду по Id.
        /// Только администратор проекта.
        /// </summary>
        Task<bool> DeleteRewardAsync(int rewardId, int userId);

        /// <summary>
        /// Получить список продуктов награды.
        /// Пользователь должен быть участником или администратором проекта.
        /// </summary>
        Task<List<ProductDto>> GetProductsByRewardAsync(int rewardId, int userId);

        /// <summary>
        /// Добавить продукт к награде.
        /// Только администратор проекта.
        /// </summary>
        Task<ProductDto> AddProductToRewardAsync(int rewardId, CreateProductDto dto, int userId);

        /// <summary>
        /// Обновить продукт награды по Id.
        /// Только администратор проекта.
        /// </summary>
        Task<ProductDto?> UpdateProductAsync(int productId, UpdateProductDto dto, int userId);

        /// <summary>
        /// Удалить продукт из награды по Id.
        /// Только администратор проекта.
        /// </summary>
        Task<bool> DeleteProductAsync(int productId, int userId);
    }
}
