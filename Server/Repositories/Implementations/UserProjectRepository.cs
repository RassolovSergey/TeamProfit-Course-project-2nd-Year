using Data.Context;
using Data.Entities;
using Server.Repositories.Implementations.GenericRepository;
using Server.Repositories.Interfaces;

namespace Server.Repositories.Implementations
{
    public class UserProjectRepository : GenericRepository<UserProject>, IUserProjectRepository
    {
        // Вызываем конструктор базового класса
        public UserProjectRepository(AppDbContext db) : base(db)
        {

        }
        public async Task<UserProject?> GetByIdsAsync(int userId, int projectId)
    => await _set.FindAsync(userId, projectId);

        public async Task DeleteByIdsAsync(int userId, int projectId)
        {
            var entity = await _set.FindAsync(userId, projectId);
            if (entity != null) _set.Remove(entity);
        }
    }
}
