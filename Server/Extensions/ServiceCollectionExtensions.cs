using System.Text;
using Data.Context;
using Data.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Server.Options;
using Server.Repositories.Implementations;
using Server.Repositories.Implementations.GenericRepository;
using Server.Repositories.Interfaces;
using Server.Repositories.Interfaces.Generic_Repository;
using Server.Services.HostedServices;
using Server.Services.Implementations;
using Server.Services.Interfaces;
using Server.Authorization;
using Microsoft.AspNetCore.Authorization;

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
            services.AddScoped<IRewardRepository, RewardRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<ISaleRepository, SaleRepository>();
            services.AddScoped<IProjectRepository, ProjectRepository>();
            services.AddScoped<ICostRepository, CostRepository>();
            services.AddScoped<ICurrencyRepository, CurrencyRepository>();
            services.AddScoped<IUserProjectRepository, UserProjectRepository>();
            services.AddScoped<IGenericRepository<UserProject>, GenericRepository<UserProject>>();
            return services;
        }

        /// <summary>
        /// Регистрирует бизнес-сервисы
        /// </summary>
        public static IServiceCollection AddBusinessServices(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IRewardService, RewardService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<ISaleService, SaleService>();
            services.AddScoped<ICostService, CostService>();
            services.AddScoped<ICurrencyService, CurrencyService>();
            services.AddScoped<IUserProjectService, UserProjectService>();
            services.AddScoped<IProjectService, ProjectService>();
            return services;
        }

        /// <summary>
        /// Регистрирует JWT-аутентификацию и авторизацию
        /// </summary>
        /// <summary>
        public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration config)
        {
            services
                // регистрируем схему аутентификации по JWT
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                // и сам обработчик bearer-токенов
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = config["Jwt:Issuer"],
                        ValidAudience = config["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(config["Jwt:Key"]))
                    };
                });

            return services;
        }

        /// <summary>
        /// Регистрирует всё, что связано с обновлением курсов валют
        /// </summary>
        public static IServiceCollection AddExchangeRateUpdater(this IServiceCollection services, IConfiguration config)
        {
            services.Configure<FixerApiOptions>(
                config.GetSection("FixerApi"));

            var section = config.GetSection("FixerApi");
            var baseUrl = section.GetValue<string>("BaseUrl");
            if (string.IsNullOrWhiteSpace(baseUrl))
                throw new Exception("BaseUrl in FixerApi config section is missing!");

            services.AddHttpClient("FixerApi", client =>
            {
                client.BaseAddress = new Uri(baseUrl);
            });

            services.AddHostedService<CurrencyRateUpdaterService>();
            return services;
        }

        /// <summary>
        /// Регистрирует политики авторизации для проектов и привязанные к ним хендлеры.
        /// </summary>
        public static IServiceCollection AddProjectAuthorization(this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy("ProjectMember", p =>
                    p.Requirements.Add(new ProjectMemberRequirement()));
                options.AddPolicy("ProjectAdmin", p =>
                    p.Requirements.Add(new ProjectAdminRequirement()));
            });

            services.AddScoped<IAuthorizationHandler, ProjectMemberHandler>();
            services.AddScoped<IAuthorizationHandler, ProjectAdminHandler>();

            return services;
        }
    }
}
