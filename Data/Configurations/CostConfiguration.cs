using Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Data.Configurations
{
    // Конфигурация сущности Cost
    public class CostConfiguration : IEntityTypeConfiguration<Cost>
    {
        public void Configure(EntityTypeBuilder<Cost> builder)
        {
            builder.ToTable("Costs");
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Amount)
                   .IsRequired();
            builder.Property(c => c.Description)
                   .HasMaxLength(500);
            builder.Property(c => c.Type)
                   .IsRequired();

            // связь n:1: Cost - Project
            builder.HasOne(c => c.Project)
                   .WithMany(p => p.Costs)
                   .HasForeignKey(c => c.ProjectId)
                   .OnDelete(DeleteBehavior.Cascade);

            // связь n:1: Cost - User
            builder.HasOne(c => c.User)
                   .WithMany(u => u.Costs)
                   .HasForeignKey(c => c.UserId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
