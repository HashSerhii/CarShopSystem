using CarShop.Application.Queries;
using CarShop.Application.DTOs;
using CarShop.Domain;

namespace CarShop.Application.Repositories;

public interface IGetCarsRepository
{
    Task<PagedResult<CarListItemModel>> GetCarsAsync(GetCarsQuery query, CancellationToken ct);
    Task<CarDetailModel?> GetCarByIdAsync(int carId, CancellationToken cancellationToken);

    Task<int> AddCarAsync(Car car, CancellationToken cancellationToken);

    Task<bool> DeleteCarAsync(int id, CancellationToken cancellationToken);

    Task<string?> GetCarOwnerIdAsync(int carId, CancellationToken cancellationToken);

    Task<bool> UpdateCarStatusAsync(int carId, ListingStatus status, CancellationToken cancellationToken);

    Task<ListingStatus?> GetCarStatusAsync(int carId, CancellationToken cancellationToken);
}