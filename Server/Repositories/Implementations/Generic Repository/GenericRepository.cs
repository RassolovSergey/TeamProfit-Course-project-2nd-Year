using Data.Context;
using Microsoft.EntityFrameworkCore;
using Server.Repositories.Interfaces.Generic_Repository;

// Выполняет SQL запросы в базе данных -  что-то вроде
namespace Server.Repositories.Implementations.GenericRepository
{
    /// <summary>
    /// Базовая реализация IGenericRepository для любой TEntity через EF Core
    /// </summary>
    /// <typeparam name="TEntity">класс-сущность</typeparam>
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        protected readonly AppDbContext _db;    // сам DbContext
        protected readonly DbSet<TEntity> _set; // таблица для TEntity

        // Конструктор
        public GenericRepository(AppDbContext db)
        {
            _db = db;                   // DbContext
            _set = db.Set<TEntity>();   // DbSet<TEntity> возвращает DbSet для нужной таблицы (Напрмиер: DbSet<Team>)
        }

        // Получить полнный список
        public virtual async Task<List<TEntity>> GetAllAsync()
        {
            // просто ToListAsync по всей таблице
            return await _set.ToListAsync();
        }

        // Получить по Id
        public virtual async Task<TEntity?> GetByIdAsync(int id)
        {
            // FindAsync умеет искать по первичному ключу
            return await _set.FindAsync(id);
        }

        // Добавить
        public virtual async Task AddAsync(TEntity entity)
        {
            // Буферизуем добавление, но не сохраняем сразу
            await _set.AddAsync(entity);
        }

        // Обновить
        public virtual Task UpdateAsync(TEntity entity)
        {
            // Помечаем сущность как изменённую
            _set.Update(entity);
            return Task.CompletedTask;
        }

        // Удалить
        public virtual async Task DeleteAsync(int id)
        {
            // Сначала находим сущность по ключу
            var entity = await _set.FindAsync(id);
            if (entity != null)
                _set.Remove(entity); // Если нашлась — удаляем
        }

        // Сохранить
        public virtual Task SaveChangesAsync()
        {
            // Применяем все накопленные Add/Update/Delete к БД
            return _db.SaveChangesAsync();
        }
    }
}
