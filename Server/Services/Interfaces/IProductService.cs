using Server.DTO.Product;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Server.Services.Interfaces
{
    public interface IProductService
    {
        /// <summary>
        /// Получить продукт по Id.
        /// </summary>
        Task<ProductDto?> GetByIdAsync(int productId);

        /// <summary>
        /// Создать продукт.
        /// </summary>
        Task<ProductDto> CreateAsync(int projectId, int rewardId, CreateProductDto dto);

        /// <summary>
        /// Обновить продукт.
        /// </summary>
        Task<ProductDto?> UpdateAsync(int productId, UpdateProductDto dto);

        /// <summary>
        /// Удалить продукт.
        /// </summary>
        Task<bool> DeleteAsync(int productId);
    }
}
