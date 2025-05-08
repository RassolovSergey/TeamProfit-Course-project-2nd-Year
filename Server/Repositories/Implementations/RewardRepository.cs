using Data.Context;
using Data.Entities;
using Server.Repositories.Implementations.GenericRepository;
using Server.Repositories.Interfaces;

namespace Server.Repositories.Implementations
{
    public class RewardRepository : GenericRepository<Reward>, IRewardRepository
    {
        // Вызываем конструктор базового класса
        public RewardRepository(AppDbContext db) : base(db)
        {
        }
    }
}
