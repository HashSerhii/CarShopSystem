using CarShop.Application.DTOs;
using CarShop.Application.Repositories;
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
}
