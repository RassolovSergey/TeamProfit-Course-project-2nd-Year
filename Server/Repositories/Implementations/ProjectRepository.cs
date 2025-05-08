using Data.Context;
using Data.Entities;
using Server.Repositories.Implementations.GenericRepository;
using Server.Repositories.Interfaces;

namespace Server.Repositories.Implementations
{
    public class ProjectRepository : GenericRepository<Project>, IProjectRepository
    {
        // Вызываем конструктор базового класса
        public ProjectRepository(AppDbContext db) : base(db)
        {
        }
    }
}
