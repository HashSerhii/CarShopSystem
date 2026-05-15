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

    public async Task<CarDetailModel?> GetCarByIdAsync(int carId, CancellationToken cancellationToken)
    {
        var car = await _db.Cars
            .AsNoTracking()
            .Include(c => c.Brand)
            .Include(c => c.Photos)
            .Where(c => c.Id == carId)
            .Select(c => new CarDetailModel(
                c.Id,
                c.Brand.Name,
                c.Model,
                c.Year,
                c.Price,
                c.Description,
                c.Photos.Select(p => p.Url).FirstOrDefault(),
                c.Photos.Select(p => p.Url).ToList(),
                c.Owner.PhoneNumber
            ))
            .FirstOrDefaultAsync(cancellationToken);
        return car;
    }

    public async Task<int> AddCarAsync(Car car, CancellationToken cancellationToken)
    {
        await _db.Cars.AddAsync(car, cancellationToken);

        await _db.SaveChangesAsync(cancellationToken);

        return car.Id;
    }

    public async Task<bool> DeleteCarAsync(int id, CancellationToken cancellationToken)
    {
        var car = await _db.Cars
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

        if (car == null)
        {
            return false;
        }

        _db.Cars.Remove(car);

        await _db.SaveChangesAsync(cancellationToken);

        return true;
    }

    public async Task<string?> GetCarOwnerIdAsync(int carId, CancellationToken cancellationToken)
    {
        return await _db.Cars
            .AsNoTracking()
            .Where(c => c.Id == carId)
            .Select(c => c.OwnerId)
            .FirstOrDefaultAsync(cancellationToken);
    }
}