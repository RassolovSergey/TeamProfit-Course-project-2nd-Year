using AutoMapper;
using Data.Entities;
using Server.DTO.Product;
using Server.Repositories.Interfaces;
using Server.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Server.Services.Implementations
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepo;
        private readonly IMapper _mapper;
        private readonly IRewardRepository _rewardRepo;

        public ProductService(IProductRepository productRepo, IMapper mapper, IRewardRepository rewardRepo)
        {
            _productRepo = productRepo;
            _mapper = mapper;
            _rewardRepo = rewardRepo;
        }

        public async Task<ProductDto?> GetByIdAsync(int productId)
        {
            var product = await _productRepo.GetByIdAsync(productId);
            if (product == null)
                return null;
            return _mapper.Map<ProductDto>(product);
        }

        public async Task<ProductDto> CreateAsync(int projectId, int rewardId, CreateProductDto dto)
        {
            // Получаем награду с продуктами
            var reward = await _rewardRepo.GetWithProductsAsync(rewardId);
            if (reward == null)
                throw new KeyNotFoundException("Reward not found");

            // Создаем продукт из DTO
            var product = _mapper.Map<Product>(dto);

            // Добавляем продукт в коллекцию награды
            reward.Products.Add(product);

            // Сохраняем изменения
            await _rewardRepo.SaveChangesAsync();

            // Возвращаем DTO созданного продукта
            return _mapper.Map<ProductDto>(product);
        }

        public async Task<ProductDto?> UpdateAsync(int productId, UpdateProductDto dto)
        {
            var product = await _productRepo.GetByIdAsync(productId);
            if (product == null)
                return null;

            _mapper.Map(dto, product);
            await _productRepo.SaveChangesAsync();

            return _mapper.Map<ProductDto>(product);
        }

        public async Task<bool> DeleteAsync(int productId)
        {
            var product = await _productRepo.GetByIdAsync(productId);
            if (product == null)
                return false;

            await _productRepo.DeleteAsync(productId);
            await _productRepo.SaveChangesAsync();

            return true;
        }
    }
}
