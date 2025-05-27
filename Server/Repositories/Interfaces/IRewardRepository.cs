// IRewardRepository.cs
using Data.Entities;
using Server.Repositories.Interfaces.Generic_Repository;

public interface IRewardRepository : IGenericRepository<Reward>
{
    /// <summary>Получить все продукты, связанные с данной наградой</summary>
    Task<Reward?> GetWithProductsAsync(int rewardId);
}
