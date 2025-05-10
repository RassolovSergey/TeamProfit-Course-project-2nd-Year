using Data.Context;
using Microsoft.EntityFrameworkCore;
using Server.Repositories.Implementations;
using Server.Repositories.Implementations.GenericRepository;
using Server.Repositories.Interfaces;
using Server.Repositories.Interfaces.Generic_Repository;
using Server.Services.Implementations;
using Server.Services.Interfaces;


namespace Server.Extensions
{
    /// <summary>
    /// Методы расширения для регистрации сервисов в DI
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Регистрирует EF Core DbContext и настраивает подключение к PostgreSQL
        /// </summary>
        public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            // ConnectionString из appsettings.json
            var connStr = configuration.GetConnectionString("DefaultConnection");
            // Регистрируем DbContext с провайдером PostgreSQL
            services.AddDbContext<AppDbContext>(options => options.UseNpgsql(connStr));

            return services;
        }

        /// <summary>
        /// Регистрирует слои репозиториев
        /// </summary>
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            // Регистрация обобщённого репозитория
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            // Регистрация для каждого интерфейса
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ITeamRepository, TeamRepository>();
            services.AddScoped<IProjectRepository, ProjectRepository>();
            services.AddScoped<IRewardRepository, RewardRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<ISaleRepository, SaleRepository>();
            services.AddScoped<ICostRepository, CostRepository>();
            services.AddScoped<ICurrencyRepository, CurrencyRepository>();
            return services;
        }

        /// <summary>
        /// Регистрирует бизнес-сервисы
        /// </summary>
        public static IServiceCollection AddBusinessServices(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ITeamService, TeamService>();
            services.AddScoped<IProjectService, ProjectService>();
            services.AddScoped<IRewardService, RewardService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<ISaleService, SaleService>();
            services.AddScoped<ICostService, CostService>();
            services.AddScoped<ICurrencyService, CurrencyService>();

            return services;
        }
    }
}
