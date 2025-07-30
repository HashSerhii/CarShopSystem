using CarShop.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarShop.Infrastructure.EntityConfigurations;

public class CarPhotoEntityConfiguration : IEntityTypeConfiguration<CarPhoto>
{
    public void Configure(EntityTypeBuilder<CarPhoto> builder)
    {
        builder.HasKey(cp => cp.Id);

        builder.HasOne(cp => cp.Car)
            .WithMany(c => c.Photos)
            .HasForeignKey(cp => cp.CarId)
            .OnDelete(DeleteBehavior.Cascade);
    }
} 