using System;
using System.Collections.Generic;
using Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Data.Configurations
{
    // Конфигурация сущности Reward
    public class RewardConfiguration : IEntityTypeConfiguration<Reward>
    {
        public void Configure(EntityTypeBuilder<Reward> builder)
        {
            builder.ToTable("Rewards");
            builder.HasKey(r => r.Id);

            builder.Property(r => r.Name)
                   .IsRequired()
                   .HasMaxLength(50);
            builder.Property(r => r.Description)
                   .HasMaxLength(500);
            builder.Property(r => r.Price)
                   .IsRequired();

            // связь n:1: Reward - Project
            builder.HasOne(r => r.Project)
                   .WithMany(p => p.Rewards)
                   .HasForeignKey(r => r.ProjectId)
                   .OnDelete(DeleteBehavior.Cascade);

            // связь 1:n: Reward - Sale
            builder.HasMany(r => r.Sales)
                   .WithOne(s => s.Reward)
                   .HasForeignKey(s => s.RewardId)
                   .OnDelete(DeleteBehavior.Cascade);

            // связь m:n: Reward - Product
            builder.HasMany(r => r.Products)
                   .WithMany(p => p.Rewards)
                   .UsingEntity<Dictionary<string, object>>("ProductRewards",
                       j => j.HasOne<Product>().WithMany().HasForeignKey("ProductId"),
                       j => j.HasOne<Reward>().WithMany().HasForeignKey("RewardId"),
                       je => { je.HasKey("RewardId", "ProductId"); je.ToTable("ProductRewards"); }
                   );
        }
    }
}
