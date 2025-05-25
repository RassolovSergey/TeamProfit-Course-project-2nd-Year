using Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Server.Extensions; // для AddControllers()
using AutoMapper;
using Server;
using Microsoft.OpenApi.Models;
using Server.Filters;



internal class Program
{
    private static void Main(string[] args)
    {
        // Создаем конструктор
        var builder = WebApplication.CreateBuilder(args);

        // Регестрация объектов
        builder.Services
               .AddDatabase(builder.Configuration)
               .AddRepositories()
               .AddBusinessServices()
               .AddJwtAuthentication(builder.Configuration)
               .AddProjectAuthorization()
               .AddAutoMapper(typeof(MappingProfile).Assembly);

        // Поддержка контроллеров Web API
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(c =>
        {
            // 1) Объявляем документацию с именем "v1" и заполняем поля info
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "TeamProfit API",
                Version = "3.1.0",
                Description = "API для курсового проекта TeamProfit"
            });

            // 2) Конфигурация схемы авторизации (у вас уже есть)
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
                    new string[]{}
                }
            });

            // 3) Ваш OperationFilter
            c.OperationFilter<HideAuthorizeOperationsFilter>();
        });


        builder.Services.AddExchangeRateUpdater(builder.Configuration);

        var app = builder.Build();


        app.UseAuthentication();
        app.UseAuthorization();

        // Настройка HTTP
        // Конфигурация пайплайна запросов
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "TeamProfit API v1");
        });


        app.MapControllers();

        app.Run();
    }
}