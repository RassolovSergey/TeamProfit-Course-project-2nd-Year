using Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Data.Configurations
{
    // Конфигурация сущности Sale
    public class SaleConfiguration : IEntityTypeConfiguration<Sale>
    {
        public void Configure(EntityTypeBuilder<Sale> builder)
        {
            builder.ToTable("Sales");
            builder.HasKey(s => s.Id);

            builder.Property(s => s.Date)
                   .IsRequired();
            builder.Property(s => s.BackerName)
                   .IsRequired();
            builder.Property(s => s.BackerEmail)
                   .IsRequired();

            // связь n:1: Sale - Reward
            builder.HasOne(s => s.Reward)
                   .WithMany(r => r.Sales)
                   .HasForeignKey(s => s.RewardId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
