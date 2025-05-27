using System.Linq.Expressions; // Позволяет использовать деревья выражений для гибких запросов.

namespace Server.Repositories.Interfaces.Generic_Repository
{
    public interface IGenericRepository<TEntity>
        where TEntity : class
    {
        Task<List<TEntity>> GetAllAsync();      // Асинхронно получает все сущности.
        Task<TEntity?> GetByIdAsync(int id);    // Асинхронно получает сущность по ID.
        Task AddAsync(TEntity entity);          // Асинхронно добавляет новую сущность.
        Task UpdateAsync(TEntity entity);       // Асинхронно обновляет сущность.
        Task DeleteAsync(int id);               // Асинхронно удаляет сущность по ID.
        Task SaveChangesAsync();                // Асинхронно сохраняет изменения в хранилище.
        Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate); // Асинхронно проверяет наличие сущностей по предикату.
    }
}