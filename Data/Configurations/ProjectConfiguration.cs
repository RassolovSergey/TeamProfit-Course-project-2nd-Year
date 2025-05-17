using Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Configurations
{
    /// <summary>
    /// Конфигурация EF Core для сущности Project
    /// </summary>
    public class ProjectConfiguration : IEntityTypeConfiguration<Project>
    {
        public void Configure(EntityTypeBuilder<Project> builder)
        {
            builder.ToTable("Projects");
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Name)
                   .IsRequired();

            builder.Property(p => p.Description)
                   .HasMaxLength(500);

            builder.Property(p => p.DateStart)
                   .IsRequired();

            builder.Property(p => p.DateClose)
                   .IsRequired();

            builder.Property(p => p.ProjectDuration)
                   .IsRequired();

            builder.Property(p => p.Status)
                   .HasConversion<string>()
                   .HasMaxLength(50)
                   .IsRequired(false);

            // Проект — награды (1:n)
            builder.HasMany(p => p.Rewards)
                   .WithOne(r => r.Project)
                   .HasForeignKey(r => r.ProjectId)
                   .OnDelete(DeleteBehavior.Cascade);

            // Проект — расходы (1:n)
            builder.HasMany(p => p.Costs)
                   .WithOne(c => c.Project)
                   .HasForeignKey(c => c.ProjectId)
                   .OnDelete(DeleteBehavior.Cascade);

            // Проект — пользователи (1:n через UserProject)
            builder.HasMany(p => p.UserProjects)
                   .WithOne(up => up.Project)
                   .HasForeignKey(up => up.ProjectId)
                   .OnDelete(DeleteBehavior.Cascade);

            // Новая связь: каждая валюта может быть у многих проектов,
            // но проект — только в одной валюте
            builder.HasOne(p => p.Currency)
                   .WithMany(c => c.Projects)
                   .HasForeignKey(p => p.CurrencyId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
