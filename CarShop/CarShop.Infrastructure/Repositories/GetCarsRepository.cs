using CarShop.Application.DTOs;
using CarShop.Application.Queries;
using CarShop.Application.Repositories;
using Microsoft.EntityFrameworkCore;
using CarShop.Domain;


namespace CarShop.Infrastructure.Repositories;

public class GetCarsRepository : IGetCarsRepository
{
    private readonly AppDbContext _db;

    public GetCarsRepository(AppDbContext db) => _db = db;

    public async Task<PagedResult<CarListItemDto>> GetCarsAsync(GetCarsQuery q, CancellationToken cancellationToken)
    {
        IQueryable<Car> query = _db.Cars.AsNoTracking();

        if (q.BrandId.HasValue) query = query.Where(c => c.BrandId == q.BrandId.Value);
        if (q.YearFrom.HasValue) query = query.Where(c => c.Year >= q.YearFrom.Value);
        if (q.YearTo.HasValue) query = query.Where(c => c.Year <= q.YearTo.Value);

        query = q.Sort switch
        {
            "price_desc" => query.OrderByDescending(c => c.Price),
            "year_asc" => query.OrderBy(c => c.Year),
            "year_desc" => query.OrderByDescending(c => c.Year),
            _ => query.OrderBy(c => c.Price)
        };

        var total = await query.CountAsync(cancellationToken);

        var items = await query
            .Skip((q.Page - 1) * q.PageSize)
            .Take(q.PageSize)
            .Select(c => new CarListItemDto(
                c.Id,
                c.Brand.Name,
                c.Model,
                c.Year,
                c.Price,
                c.Photos.FirstOrDefault() != null ? c.Photos.FirstOrDefault()!.Url : null
            ))
            .ToListAsync(cancellationToken);

        return new PagedResult<CarListItemDto>(items, total, q.Page, q.PageSize);
    }
}