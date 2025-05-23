// RewardRepository.cs
using Data.Context;
using Data.Entities;
using Microsoft.EntityFrameworkCore;
using Server.Repositories.Implementations.GenericRepository;

public class RewardRepository : GenericRepository<Reward>, IRewardRepository
{
    private readonly AppDbContext _db;
    public RewardRepository(AppDbContext db) : base(db) => _db = db;

    public async Task<Reward?> GetWithProductsAsync(int rewardId)
    {
        return await _db.Rewards
                        .Include(r => r.Products)
                        .FirstOrDefaultAsync(r => r.Id == rewardId);
    }
}
