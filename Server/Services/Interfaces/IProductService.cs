// Server/Services/Interfaces/IProductService.cs
using Server.DTO.Product;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Server.Services.Interfaces
{
    /// <summary>Бизнес-логика для работы с продуктами</summary>
    public interface IProductService
    {
        Task<List<ProductDto>> GetAllAsync();
        Task<ProductDto?> GetByIdAsync(int id);
        Task<ProductDto> CreateAsync(CreateProductDto dto);
        Task<ProductDto?> UpdateAsync(int id, UpdateProductDto dto);
        Task<bool> DeleteAsync(int id);
        Task<List<ProductDto>> GetByRewardAsync(int rewardId);
    }
}
