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

        var existing = await db.Brands
            .Select(b => b.Name.ToLower())
            .ToListAsync();

        var toAdd = DefaultBrands
            .Where(name => !existing.Contains(name.ToLower()))
            .Select(name => new Brand { Name = name })
            .ToList();

        if (toAdd.Count > 0)
        {
            await db.Brands.AddRangeAsync(toAdd);
            await db.SaveChangesAsync();
        }
    }
}
