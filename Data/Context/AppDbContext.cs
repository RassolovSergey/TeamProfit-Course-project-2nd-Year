using Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data.Context
{
    public  class AppDbContext : DbContext
    {
        // Конструктор принимает опции(строка подключения, провайдер) и передаёт их в базовый DbContext
        public AppDbContext(DbContextOptions<AppDbContext> options) :base(options) { }

        // Наборы сущностей — приводятся к таблицам в БД
        public DbSet<Team> Teams { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<UserProject> UserProjects { get; set; }
        public DbSet<Reward> Rewards { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Sale> Sales { get; set; }
        public DbSet<Cost> Costs { get; set; }
        public DbSet<Currency> Currencies { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Найдёт и применит все классы из Data/Configurations
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

            base.OnModelCreating(modelBuilder);
        }
    }
}
