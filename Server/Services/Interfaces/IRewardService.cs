// Server/Services/Interfaces/IRewardService.cs
using Server.DTO.Reward;

namespace Server.Services.Interfaces
{
    /// <summary>
    /// Сервис для бизнес-логики, связанной с наградами
    /// </summary>
    public interface IRewardService
    {
        Task<List<RewardDto>> GetAllAsync();
        Task<List<RewardDto>> GetByProjectAsync(int projectId);
        Task<RewardDto?> GetByIdAsync(int id);

        // ← изменили: теперь принимаем projectId
        Task<RewardDto> CreateAsync(CreateRewardDto dto, int projectId);

        Task<RewardDto?> UpdateAsync(int id, UpdateRewardDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
