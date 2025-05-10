using Server.DTO.Product;
using Server.Services.Interfaces;

namespace Server.Services.Implementations
{
    public class ProductService : IProductService
    {
        public Task<ProductDto> CreateAsync(CreateProductDto dto)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<ProductDto>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<ProductDto?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ProductDto?> UpdateAsync(int id, UpdateProductDto dto)
        {
            throw new NotImplementedException();
        }
    }
}
