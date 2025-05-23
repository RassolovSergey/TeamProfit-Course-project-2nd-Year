// Server/Services/Implementations/RewardService.cs
using AutoMapper;
using Data.Entities;
using Server.DTO.Reward;
using Server.Repositories.Interfaces;
using Server.Services.Interfaces;

namespace Server.Services.Implementations
{
    /// <summary>Реализация сервиса для работы с наградами</summary>
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
            var list = await _repo.GetAllAsync();
            return _mapper.Map<List<RewardDto>>(list);
        }

        public async Task<List<RewardDto>> GetByProjectAsync(int projectId)
        {
            var list = await _repo.GetAllAsync();
            return _mapper
                .Map<List<RewardDto>>(list.Where(r => r.ProjectId == projectId).ToList());
        }

        public async Task<RewardDto?> GetByIdAsync(int id)
        {
            var ent = await _repo.GetByIdAsync(id);
            return ent is null ? null : _mapper.Map<RewardDto>(ent);
        }

        public async Task<RewardDto> CreateAsync(CreateRewardDto dto, int projectId)
        {
            // мапим из DTO
            var reward = _mapper.Map<Reward>(dto);

            // ставим связь на проект
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
    }
}
