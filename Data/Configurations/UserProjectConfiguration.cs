using Data.Entities;
using Data.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Configurations
{
    /// <summary>
    /// Конфигурация EF Core для сущности UserProject
    /// </summary>
    public class UserProjectConfiguration : IEntityTypeConfiguration<UserProject>
    {
        public void Configure(EntityTypeBuilder<UserProject> builder)
        {
            builder.ToTable("UserProjects");

            // Составной ключ: UserId + ProjectId
            builder.HasKey(up => new { up.UserId, up.ProjectId });

            // Связь с пользователем
            builder.HasOne(up => up.User)
                   .WithMany(u => u.UserProjects)
                   .HasForeignKey(up => up.UserId)
                   .OnDelete(DeleteBehavior.Cascade);

            // Связь с проектом
            builder.HasOne(up => up.Project)
                   .WithMany(p => p.UserProjects)
                   .HasForeignKey(up => up.ProjectId)
                   .OnDelete(DeleteBehavior.Cascade);

            // Тип сотрудничества — сохраняем как строку (enum)
            builder.Property(up => up.TypeCooperation)
                   .HasConversion<string>()
                   .HasMaxLength(50)
                   .IsRequired();

            // Фиксированная сумма — обязательна
            builder.Property(up => up.FixedPrice)
                   .HasColumnType("numeric")
                   .IsRequired();

            // Процент от прибыли — обязательна
            builder.Property(up => up.PercentPrice)
                   .HasColumnType("numeric")
                   .IsRequired();

            // Флаг администратора проекта — обязательна
            builder.Property(up => up.IsAdmin)
                   .IsRequired();

            // Уникальный индекс для обеспечения одного администратора на проект
            builder.HasIndex(up => up.ProjectId)
                   .IsUnique()
                   .HasFilter("\"IsAdmin\" = TRUE")
                   .HasDatabaseName("IX_UserProjects_Project_Admin");
        }
    }
}
