using Server.DTO.Reward;
using Server.Repositories.Interfaces;
using Server.Services.Interfaces;

namespace Server.Services.Implementations
{
    public class RewardService : IRewardService
    {
        public Task<RewardDto> CreateAsync(CreateRewardDto dto)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<RewardDto>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<RewardDto?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<RewardDto?> UpdateAsync(int id, UpdateRewardDto dto)
        {
            throw new NotImplementedException();
        }
    }
}
