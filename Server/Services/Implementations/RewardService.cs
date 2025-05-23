// Server/Services/Implementations/RewardService.cs
using AutoMapper;
using Data.Entities;
using Server.DTO.Reward;
using Server.DTO.Product;
using Server.Repositories.Interfaces;
using Server.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Services.Implementations
{
    public class RewardService : IRewardService
    {
        private readonly IRewardRepository _repo;
        private readonly IProductRepository _prodRepo;
        private readonly IMapper _mapper;

        public RewardService(
            IRewardRepository repo,
            IProductRepository prodRepo,
            IMapper mapper)
        {
            _repo = repo;
            _prodRepo = prodRepo;
            _mapper = mapper;
        }

        public async Task<List<RewardDto>> GetAllAsync()
        {
            var list = await _repo.GetAllAsync();
            return _mapper.Map<List<RewardDto>>(list);
        }

        public async Task<List<RewardDto>> GetByProjectAsync(int projectId)
        {
            var list = await _repo.GetAllAsync();
            return _mapper.Map<List<RewardDto>>(list.Where(r => r.ProjectId == projectId).ToList());
        }

        public async Task<RewardDto?> GetByIdAsync(int id)
        {
            var ent = await _repo.GetByIdAsync(id);
            return ent is null ? null : _mapper.Map<RewardDto>(ent);
        }

        public async Task<RewardDto> CreateAsync(CreateRewardDto dto, int projectId)
        {
            var reward = _mapper.Map<Reward>(dto);
            reward.ProjectId = projectId;
            await _repo.AddAsync(reward);
            await _repo.SaveChangesAsync();
            return _mapper.Map<RewardDto>(reward);
        }

        public async Task<RewardDto?> UpdateAsync(int id, UpdateRewardDto dto)
        {
            var reward = await _repo.GetByIdAsync(id);
            if (reward == null) return null;
            _mapper.Map(dto, reward);
            await _repo.UpdateAsync(reward);
            await _repo.SaveChangesAsync();
            return _mapper.Map<RewardDto>(reward);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var exists = await _repo.GetByIdAsync(id) != null;
            if (!exists) return false;
            await _repo.DeleteAsync(id);
            await _repo.SaveChangesAsync();
            return true;
        }

        public async Task<bool> AddProductAsync(int rewardId, int productId)
        {
            var reward = await _repo.GetWithProductsAsync(rewardId);
            if (reward == null) return false;

            var product = await _prodRepo.GetByIdAsync(productId);
            if (product == null) return false;

            if (!reward.Products.Any(p => p.Id == productId))
                reward.Products.Add(product);

            await _repo.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveProductAsync(int rewardId, int productId)
        {
            var reward = await _repo.GetWithProductsAsync(rewardId);
            if (reward == null) return false;

            var prod = reward.Products.FirstOrDefault(p => p.Id == productId);
            if (prod == null) return false;

            reward.Products.Remove(prod);
            await _repo.SaveChangesAsync();
            return true;
        }

        public async Task<List<ProductDto>> GetProductsByRewardAsync(int rewardId)
        {
            var reward = await _repo.GetWithProductsAsync(rewardId);
            if (reward == null) return new List<ProductDto>();
            return reward.Products
                         .Select(p => _mapper.Map<ProductDto>(p))
                         .ToList();
        }
    }
}
