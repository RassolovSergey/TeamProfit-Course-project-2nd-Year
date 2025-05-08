using Data.Context;
using Data.Entities;
using Microsoft.EntityFrameworkCore;
using Server.Repositories.Implementations.GenericRepository;
using Server.Repositories.Interfaces;

namespace Server.Repositories.Implementations
{
    /// <summary>
    /// Конкретный репозиторий для User,
    /// Наследует весь CRUD из GenericRepository,
    /// Можно добавить специфичные методы.
    /// </summary>
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        // Вызываем конструктор базового класса
        public UserRepository(AppDbContext db) : base(db)
        {
        }
    }
}
