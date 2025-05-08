using Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Server.Extensions; // ��� AddControllers()

internal class Program
{
    private static void Main(string[] args)
    {
        // ������� �����������
        var builder = WebApplication.CreateBuilder(args);

        // ����������� ��������
        builder.Services
               .AddDatabase(builder.Configuration)
               .AddRepositories()
               .AddBusinessServices();

        // ��������� ������������ Web API
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        // ��������� HTTP
        // ������������ ��������� ��������
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