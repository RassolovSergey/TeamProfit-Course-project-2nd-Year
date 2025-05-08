using Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Server.Extensions; // для AddControllers()

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
               .AddBusinessServices();

        // Поддержка контроллеров Web API
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        // Настройка HTTP
        // Конфигурация пайплайна запросов
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}