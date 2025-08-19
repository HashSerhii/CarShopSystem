using CarShop.Application.DTOs;
using CarShop.Application.Repositories;
using CarShop.Domain;
using Microsoft.EntityFrameworkCore;

namespace CarShop.Infrastructure.Repositories;

public class FavoritesRepository(AppDbContext db) : IAddFavoriteRepository, IGetFavoritesRepository
{
	public async Task AddFavoriteAsync(string userId, int carId, CancellationToken ct)
	{
		var exists = await db.Set<FavoriteCar>().AnyAsync(x => x.UserId == userId && x.CarId == carId, ct);
		if (exists) return;
		db.Set<FavoriteCar>().Add(new FavoriteCar { UserId = userId, CarId = carId });
		await db.SaveChangesAsync(ct);
	}

	public async Task<PagedResult<FavoriteModel>> GetFavoritesAsync(string userId, int page, int pageSize, CancellationToken ct)
	{
		var query = db.Set<FavoriteCar>().AsNoTracking().Where(f => f.UserId == userId);
		var total = await query.CountAsync(ct);
		var items = await query
			.OrderBy(f => f.CarId)
			.Skip((page - 1) * pageSize)
			.Take(pageSize)
			.Select(f => new FavoriteModel(
				f.CarId,
				f.Car.Brand.Name,
				f.Car.Model,
				f.Car.Year,
				f.Car.Price,
				f.Car.Photos.FirstOrDefault() != null ? f.Car.Photos.FirstOrDefault()!.Url : null
			))
			.ToListAsync(ct);

		return new PagedResult<FavoriteModel>(items, total, page, pageSize);
	}
} 