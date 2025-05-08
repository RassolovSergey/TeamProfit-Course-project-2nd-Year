using Data.Context;
using Data.Entities;
using Server.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Server.Repositories.Implementations.GenericRepository;


namespace Server.Repositories.Implementations
{
    /// <summary>
    /// Конкретный репозиторий для Team,
    /// Наследует весь CRUD из GenericRepository,
    /// Можно добавить специфичные методы.
    /// </summary>
    public class TeamRepository: GenericRepository<Team>, ITeamRepository
    {
        // Вызываем конструктор базового класса
        public TeamRepository(AppDbContext db) : base(db)
        {
        }
    }
}
