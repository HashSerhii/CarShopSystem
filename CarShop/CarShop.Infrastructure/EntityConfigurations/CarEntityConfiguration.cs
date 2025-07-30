using CarShop.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarShop.Infrastructure.EntityConfigurations;

public class CarEntityConfiguration: IEntityTypeConfiguration<Car>
{
    public void Configure(EntityTypeBuilder<Car> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Model)
            .IsRequired() 
            .HasMaxLength(100); 

        builder.Property(c => c.Year)
            .IsRequired(); 

        builder.Property(c => c.Price)
            .HasColumnType("decimal(18,2)"); 

        builder.Property(c => c.Description)
            .HasMaxLength(500); 

        builder.HasOne(c => c.Brand)
            .WithMany(b => b.Cars)
            .HasForeignKey(c => c.BrandId)
            .OnDelete(DeleteBehavior.Cascade); 

        builder.HasOne(c => c.Owner)
            .WithMany(u => u.Cars)
            .HasForeignKey(c => c.OwnerId)
            .OnDelete(DeleteBehavior.SetNull); 

        builder.HasMany(c => c.FavoriteByUsers)
            .WithOne(fc => fc.Car)
            .HasForeignKey(fc => fc.CarId)
            .OnDelete(DeleteBehavior.Cascade);
    }
} 