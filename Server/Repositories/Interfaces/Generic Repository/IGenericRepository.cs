using System.Linq.Expressions;

namespace Server.Repositories.Interfaces.Generic_Repository
{
    /// <summary>
    /// Общий CRUD-контракт для любой сущности
    /// </summary>
    public interface IGenericRepository<TEntity>
        where TEntity : class
    {
        Task<List<TEntity>> GetAllAsync();
        Task<TEntity?> GetByIdAsync(int id);
        Task AddAsync(TEntity entity);
        Task UpdateAsync(TEntity entity);
        Task DeleteAsync(int id);
        Task SaveChangesAsync();
        Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate);
    }
}
