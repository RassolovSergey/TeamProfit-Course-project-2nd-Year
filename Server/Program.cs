using Server.Extensions;
using Server;
using Microsoft.OpenApi.Models;
using Server.Filters;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Регистрация сервисов в DI
        builder.Services
               .AddDatabase(builder.Configuration)               // Настройка БД
               .AddRepositories()                                // Репозитории
               .AddBusinessServices()                            // Бизнес-сервисы
               .AddJwtAuthentication(builder.Configuration)      // <— JWT-аутентификация
               .AddProjectAuthorization()                        // Политики ProjectMember / ProjectAdmin
               .AddAutoMapper(typeof(MappingProfile).Assembly)  // AutoMapper
               .AddControllers();                               // Контроллеры API

        builder.Services.AddEndpointsApiExplorer();           // Endpoints for Swagger

        // Swagger/OpenAPI
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "TeamProfit API",
                Version = "v1.0",
                Description = "API для курсового проекта TeamProfit"
            });

            // Схема Bearer
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Введите JWT как: Bearer {token}",
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id   = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });

            // Фильтр для скрытия [Authorize]-методов
            c.OperationFilter<HideAuthorizeOperationsFilter>();
        });

        // builder.Services.AddExchangeRateUpdater(builder.Configuration); // отключена фоновая служба

        var app = builder.Build();

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "TeamProfit API v1");
        });

        app.MapControllers();

        app.Run();
    }
}
