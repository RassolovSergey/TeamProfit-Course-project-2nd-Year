using Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Data.Configurations
{
    // Конфигурация сущности User
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");
            builder.HasKey(u => u.Id);

            builder.Property(u => u.Login)
                   .IsRequired()
                   .HasMaxLength(20);
            builder.Property(u => u.Email)
                   .IsRequired()
                   .HasMaxLength(40);
            builder.Property(u => u.HashPassword)
                   .IsRequired();
            builder.Property(u => u.PasswordSalt)
                   .IsRequired();

            // связь m:n: User - Team (команды участника) описана в TeamConfiguration

            // связь n:1: User - Currency
            builder.HasOne(u => u.Currency)
                   .WithMany(c => c.Users)
                   .HasForeignKey(u => u.CurrencyId)
                   .OnDelete(DeleteBehavior.Restrict);

            // связь 1:n: User - Cost
            builder.HasMany(u => u.Costs)
                   .WithOne(c => c.User)
                   .HasForeignKey(c => c.UserId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
