using CarShop.Application.DTOs;
using CarShop.Application.Queries;
using CarShop.Application.Repositories;
using Microsoft.EntityFrameworkCore;
using CarShop.Domain;
using CarShop.Application.Constants;


namespace CarShop.Infrastructure.Repositories;

public class GetCarsRepository : IGetCarsRepository
{
    private readonly AppDbContext _db;

    public GetCarsRepository(AppDbContext db) => _db = db;

    public async Task<PagedResult<CarListItemModel>> GetCarsAsync(GetCarsQuery q, CancellationToken cancellationToken)
    {
        IQueryable<Car> query = _db.Cars.AsNoTracking();

        if (q.BrandId.HasValue) query = query.Where(c => c.BrandId == q.BrandId.Value);
        if (q.YearFrom.HasValue) query = query.Where(c => c.Year >= q.YearFrom.Value);
        if (q.YearTo.HasValue) query = query.Where(c => c.Year <= q.YearTo.Value);

        query = q.Sort switch
        {
            CarSort.PriceDesc => query.OrderByDescending(c => c.Price),
            CarSort.YearAsc => query.OrderBy(c => c.Year),
            CarSort.YearDesc => query.OrderByDescending(c => c.Year),
            _ => query.OrderBy(c => c.Price)
        };

        var total = await query.CountAsync(cancellationToken);

        var items = await query
            .Skip((q.Page - 1) * q.PageSize)
            .Take(q.PageSize)
            .Select(c => new CarListItemModel(
                c.Id,
                c.Brand.Name,
                c.Model,
                c.Year,
                c.Price,
                c.Photos.FirstOrDefault() != null ? c.Photos.FirstOrDefault()!.Url : null
            ))
            .ToListAsync(cancellationToken);

        return new PagedResult<CarListItemModel>(items, total, q.Page, q.PageSize);
    }
}