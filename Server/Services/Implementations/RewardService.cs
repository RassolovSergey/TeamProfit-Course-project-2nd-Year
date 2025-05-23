// Server/Services/Implementations/RewardService.cs
using AutoMapper;
using Data.Entities;
using Server.DTO.Reward;
using Server.Repositories.Interfaces;
using Server.Services.Interfaces;

namespace Server.Services.Implementations
{
    public class RewardService : IRewardService
    {
        private readonly IRewardRepository _repo;
        private readonly IMapper _mapper;

        public RewardService(IRewardRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<List<RewardDto>> GetAllAsync()
        {
            var rewards = await _repo.GetAllAsync();
            return _mapper.Map<List<RewardDto>>(rewards);
        }

        public async Task<List<RewardDto>> GetByProjectAsync(int projectId)
        {
            var rewards = await _repo.GetAllAsync();
            return _mapper.Map<List<RewardDto>>(rewards.Where(r => r.ProjectId == projectId).ToList());
        }

        public async Task<RewardDto?> GetByIdAsync(int id)
        {
            var reward = await _repo.GetByIdAsync(id);
            return reward == null ? null : _mapper.Map<RewardDto>(reward);
        }

        public async Task<RewardDto> CreateAsync(CreateRewardDto dto)
        {
            var reward = _mapper.Map<Reward>(dto);
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
    }
}
