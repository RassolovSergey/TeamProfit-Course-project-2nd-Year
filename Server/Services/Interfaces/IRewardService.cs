using Server.DTO.Reward;
using Server.DTO.Sale;

namespace Server.Services.Interfaces
{
    public interface IRewardService
    {
        /// <summary>Получить список всех вознаграждений</summary>
        Task<List<RewardDto>> GetAllAsync();

        /// <summary>Получить список всех вознаграждений проекта</summary>
        Task<List<RewardDto>> GetByProjectAsync(int projectId);

        /// <summary>Получить вознаграждение по идентификатору или null, если не найдено</summary>
        Task<RewardDto?> GetByIdAsync(int id);

        /// <summary>Создать новое вознаграждение и вернуть его DTO</summary>
        Task<RewardDto> CreateAsync(CreateRewardDto dto);

        /// <summary>Обновить существующее вознаграждение, вернуть обновлённый DTO или null, если не найдено</summary>
        Task<RewardDto?> UpdateAsync(int id, UpdateRewardDto dto);

        /// <summary>Удалить вознаграждение по идентификатору, вернуть true, если успешно</summary>
        Task<bool> DeleteAsync(int id);
    }
}
