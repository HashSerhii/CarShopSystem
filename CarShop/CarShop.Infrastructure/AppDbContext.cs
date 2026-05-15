using CarShop.Domain;
using CarShop.Infrastructure.EntityConfigurations;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CarShop.Infrastructure;

public class AppDbContext : IdentityDbContext<User>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Brand> Brands { get; set; }
    public DbSet<Car> Cars { get; set; }
    public DbSet<CarPhoto> CarPhotos { get; set; }
    public DbSet<FavoriteCar> FavoriteCars { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfiguration(new BrandEntityConfiguration());
        builder.ApplyConfiguration(new CarEntityConfiguration());
        builder.ApplyConfiguration(new CarPhotoEntityConfiguration());
        builder.ApplyConfiguration(new FavoriteCarEntityConfiguration());
        builder.ApplyConfiguration(new UserEntityConfiguration());
    }
}