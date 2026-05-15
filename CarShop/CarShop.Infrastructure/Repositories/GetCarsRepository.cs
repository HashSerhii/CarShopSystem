using CarShop.Application.Constants;
using CarShop.Application.DTOs;
using CarShop.Application.Queries;
using CarShop.Application.Repositories;
using CarShop.Domain;
using Microsoft.EntityFrameworkCore;

namespace CarShop.Infrastructure.Repositories;

public class GetCarsRepository : IGetCarsRepository
{
    private readonly AppDbContext _db;

    public GetCarsRepository(AppDbContext db) => _db = db;

    public async Task<PagedResult<CarListItemModel>> GetCarsAsync(GetCarsQuery q, CancellationToken cancellationToken)
    {
        IQueryable<Car> query = _db.Cars.AsNoTracking().Include(c => c.Brand);

        if (q.Status.HasValue)
        {
            query = query.Where(c => c.Status == q.Status.Value);
        }
        else if (q.OnlyApproved)
        {
            query = query.Where(c => c.Status == ListingStatus.Approved);
        }

        if (q.BrandId.HasValue) query = query.Where(c => c.BrandId == q.BrandId.Value);
        if (q.YearFrom.HasValue) query = query.Where(c => c.Year >= q.YearFrom.Value);
        if (q.YearTo.HasValue) query = query.Where(c => c.Year <= q.YearTo.Value);
        if (q.PriceFrom.HasValue) query = query.Where(c => c.Price >= q.PriceFrom.Value);
        if (q.PriceTo.HasValue) query = query.Where(c => c.Price <= q.PriceTo.Value);
        if (q.MileageFrom.HasValue) query = query.Where(c => c.Mileage >= q.MileageFrom.Value);
        if (q.MileageTo.HasValue) query = query.Where(c => c.Mileage <= q.MileageTo.Value);
        if (!string.IsNullOrWhiteSpace(q.Model))
        {
            var model = q.Model.Trim().ToLower();
            query = query.Where(c => c.Model.ToLower().Contains(model));
        }

        if (!string.IsNullOrEmpty(q.OwnerId)) query = query.Where(c => c.OwnerId == q.OwnerId);

        query = q.Sort switch
        {
            CarSort.PriceDesc => query.OrderByDescending(c => c.Price),
            CarSort.YearAsc => query.OrderBy(c => c.Year),
            CarSort.YearDesc => query.OrderByDescending(c => c.Year),
            CarSort.MileageAsc => query.OrderBy(c => c.Mileage),
            CarSort.MileageDesc => query.OrderByDescending(c => c.Mileage),
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
                c.Mileage,
                c.Status.ToString(),
                c.Photos.Select(p => p.Url).FirstOrDefault()
            ))
            .ToListAsync(cancellationToken);

        return new PagedResult<CarListItemModel>(items, total, q.Page, q.PageSize);
    }

    public async Task<CarDetailModel?> GetCarByIdAsync(int carId, CancellationToken cancellationToken)
    {
        return await _db.Cars
            .AsNoTracking()
            .Where(c => c.Id == carId)
            .Select(c => new CarDetailModel(
                c.Id,
                c.Brand.Name,
                c.Model,
                c.Year,
                c.Price,
                c.Mileage,
                c.Status.ToString(),
                c.Description,
                c.Photos.Select(p => p.Url).FirstOrDefault(),
                c.Photos.Select(p => p.Url).ToList(),
                c.Owner.PhoneNumber
            ))
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<int> AddCarAsync(Car car, CancellationToken cancellationToken)
    {
        await _db.Cars.AddAsync(car, cancellationToken);
        await _db.SaveChangesAsync(cancellationToken);
        return car.Id;
    }

    public async Task<bool> DeleteCarAsync(int id, CancellationToken cancellationToken)
    {
        var car = await _db.Cars.FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
        if (car == null)
        {
            return false;
        }

        _db.Cars.Remove(car);
        await _db.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<string?> GetCarOwnerIdAsync(int carId, CancellationToken cancellationToken) =>
        await _db.Cars
            .AsNoTracking()
            .Where(c => c.Id == carId)
            .Select(c => c.OwnerId)
            .FirstOrDefaultAsync(cancellationToken);

    public async Task<bool> UpdateCarStatusAsync(int carId, ListingStatus status, CancellationToken cancellationToken)
    {
        var car = await _db.Cars.FirstOrDefaultAsync(c => c.Id == carId, cancellationToken);
        if (car is null)
        {
            return false;
        }

        car.Status = status;
        await _db.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<ListingStatus?> GetCarStatusAsync(int carId, CancellationToken cancellationToken) =>
        await _db.Cars
            .AsNoTracking()
            .Where(c => c.Id == carId)
            .Select(c => (ListingStatus?)c.Status)
            .FirstOrDefaultAsync(cancellationToken);
}
