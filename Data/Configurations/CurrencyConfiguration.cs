using Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Data.Configurations
{
    // Конфигурация сущности Currency
    public class CurrencyConfiguration : IEntityTypeConfiguration<Currency>
    {
        public void Configure(EntityTypeBuilder<Currency> builder)
        {
            builder.ToTable("Currencies");
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Code)
                   .IsRequired()
                   .HasMaxLength(3);
            builder.Property(c => c.Name)
                   .IsRequired()
                   .HasMaxLength(100);
            builder.Property(c => c.Symbol)
                   .HasMaxLength(5);
        }
    }
}
