// IRewardRepository.cs
using Data.Entities;
using Server.Repositories.Interfaces.Generic_Repository;

public interface IRewardRepository : IGenericRepository<Reward>
{
    Task<Reward?> GetWithProductsAsync(int rewardId);
}
