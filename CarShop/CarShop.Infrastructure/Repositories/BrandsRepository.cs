using CarShop.Application.DTOs;
using CarShop.Application.Repositories;
using CarShop.Domain;
using Microsoft.EntityFrameworkCore;

namespace CarShop.Infrastructure.Repositories;

public class BrandsRepository(AppDbContext db) : IGetBrandsRepository
{
    public async Task<IReadOnlyList<BrandModel>> GetBrandsAsync(CancellationToken cancellationToken) =>
        await db.Brands
            .AsNoTracking()
            .OrderBy(b => b.Name)
            .Select(b => new BrandModel(b.Id, b.Name))
            .ToListAsync(cancellationToken);

    public async Task<BrandModel> AddBrandAsync(string name, CancellationToken cancellationToken)
    {
        var normalized = name.Trim();
        var exists = await db.Brands.AnyAsync(
            b => b.Name.ToLower() == normalized.ToLower(),
            cancellationToken);

        if (exists)
        {
            throw new InvalidOperationException($"Brand '{normalized}' already exists.");
        }

        var brand = new Brand { Name = normalized };
        await db.Brands.AddAsync(brand, cancellationToken);
        await db.SaveChangesAsync(cancellationToken);
        return new BrandModel(brand.Id, brand.Name);
    }
}
