using CarShop.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CarShop.Infrastructure;

public static class DataSeeder
{
    private static readonly string[] DefaultBrands =
    [
        "Toyota", "BMW", "Mercedes-Benz", "Audi", "Volkswagen",
        "Ford", "Honda", "Hyundai", "Kia", "Nissan", "Mazda", "Skoda"
    ];

    public static async Task SeedAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        if (!await db.Brands.AnyAsync())
        {
            await db.Brands.AddRangeAsync(
                DefaultBrands.Select(name => new Brand { Name = name }));
            await db.SaveChangesAsync();
        }
    }
}
