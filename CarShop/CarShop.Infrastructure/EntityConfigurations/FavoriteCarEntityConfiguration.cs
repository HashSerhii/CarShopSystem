using CarShop.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarShop.Infrastructure.EntityConfigurations;

public class FavoriteCarEntityConfiguration : IEntityTypeConfiguration<FavoriteCar>
{
    public void Configure(EntityTypeBuilder<FavoriteCar> builder)
    {
        builder.HasKey(fc => new { fc.UserId, fc.CarId });

        builder.HasOne(fc => fc.User)
            .WithMany(u => u.FavoriteCars)
            .HasForeignKey(fc => fc.UserId)
            .OnDelete(DeleteBehavior.Cascade); 

        builder.HasOne(fc => fc.Car)
            .WithMany(c => c.FavoriteByUsers)
            .HasForeignKey(fc => fc.CarId)
            .OnDelete(DeleteBehavior.Cascade);
    }
} 