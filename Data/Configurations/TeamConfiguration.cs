using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Data.Entities;

namespace Data.Configurations
{
    // Конфигурация сущности Team
    public class TeamConfiguration : IEntityTypeConfiguration<Team>
    {
        public void Configure(EntityTypeBuilder<Team> builder)
        {
            builder.ToTable("Teams");                    // имя таблицы
            builder.HasKey(t => t.Id);                     // первичный ключ

            builder.Property(t => t.Name)                  // настройка поля Name
                   .IsRequired()
                   .HasMaxLength(20);

            // связь m:n: Team - User (участники)
            builder.HasMany(t => t.Users)
                   .WithMany(u => u.Teams)
                   .UsingEntity<Dictionary<string, object>>(  // промежуточная таблица
                       "UserTeams",
                       j => j.HasOne<User>().WithMany().HasForeignKey("UserId"),
                       j => j.HasOne<Team>().WithMany().HasForeignKey("TeamId"),
                       je =>
                       {
                           je.HasKey("TeamId", "UserId");
                           je.ToTable("UserTeams");
                       }
                   );

            // связь 1:1: Team - Project
            builder.HasOne(t => t.Project)
                   .WithOne(p => p.Team)
                   .HasForeignKey<Project>(p => p.TeamId)
                   .OnDelete(DeleteBehavior.Cascade);

            // администратор команды (без обратной связи)
            builder.HasOne(t => t.Admin)
                   .WithMany()
                   .HasForeignKey(t => t.AdminId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }

}
