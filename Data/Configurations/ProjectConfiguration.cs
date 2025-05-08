using Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Data.Configurations
{
    // Конфигурация сущности Project
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
            builder.Property(p => p.DateCreation)
                   .IsRequired();
            builder.Property(p => p.DateUpdate)
                   .IsRequired();
            builder.Property(p => p.DateClose);

            // enum статус как строка
            builder.Property(p => p.Status)
                   .HasConversion<string>()
                   .HasMaxLength(50);

            // связь 1:n: Project - Reward
            builder.HasMany(p => p.Rewards)
                   .WithOne(r => r.Project)
                   .HasForeignKey(r => r.ProjectId)
                   .OnDelete(DeleteBehavior.Cascade);

            // связь 1:n: Project - Cost
            builder.HasMany(p => p.Costs)
                   .WithOne(c => c.Project)
                   .HasForeignKey(c => c.ProjectId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
