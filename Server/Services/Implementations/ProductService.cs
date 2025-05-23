// Server/Services/Implementations/ProductService.cs
using AutoMapper;
using Data.Entities;
using Server.DTO.Product;
using Server.Repositories.Interfaces;
using Server.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Services.Implementations
{
    /// <summary>Реализация сервиса продуктов</summary>
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repo;
        private readonly IMapper _mapper;

        public ProductService(IProductRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<List<ProductDto>> GetAllAsync()
        {
            var entities = await _repo.GetAllAsync();
            return entities.Select(e =>
            {
                var dto = _mapper.Map<ProductDto>(e);
                dto.RewardIds = e.Rewards.Select(r => r.Id).ToList();
                return dto;
            }).ToList();
        }

        public async Task<ProductDto?> GetByIdAsync(int id)
        {
            var ent = await _repo.GetByIdAsync(id);
            if (ent == null) return null;
            var dto = _mapper.Map<ProductDto>(ent);
            dto.RewardIds = ent.Rewards.Select(r => r.Id).ToList();
            return dto;
        }

        public async Task<ProductDto> CreateAsync(CreateProductDto dto)
        {
            var ent = _mapper.Map<Product>(dto);
            await _repo.AddAsync(ent);
            await _repo.SaveChangesAsync();
            return _mapper.Map<ProductDto>(ent);
        }

        public async Task<ProductDto?> UpdateAsync(int id, UpdateProductDto dto)
        {
            var ent = await _repo.GetByIdAsync(id);
            if (ent == null) return null;
            _mapper.Map(dto, ent);
            await _repo.UpdateAsync(ent);
            await _repo.SaveChangesAsync();
            return _mapper.Map<ProductDto>(ent);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var exists = await _repo.GetByIdAsync(id) != null;
            if (!exists) return false;
            await _repo.DeleteAsync(id);
            await _repo.SaveChangesAsync();
            return true;
        }

        public async Task<List<ProductDto>> GetByRewardAsync(int rewardId)
        {
            var list = await _repo.GetByRewardIdAsync(rewardId);
            return list.Select(ent => _mapper.Map<ProductDto>(ent)).ToList();
        }
    }
}
