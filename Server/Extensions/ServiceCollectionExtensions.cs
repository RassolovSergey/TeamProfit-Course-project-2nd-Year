using Data.Context;
using Microsoft.EntityFrameworkCore;

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

            return services;
        }

        /// <summary>
        /// Регистрирует бизнес-сервисы
        /// </summary>
        public static IServiceCollection AddBusinessServices(this IServiceCollection services)
        {

            return services;
        }
    }
}
