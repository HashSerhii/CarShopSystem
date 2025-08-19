using CarShop.Application.Queries;
using CarShop.Application.DTOs;

namespace CarShop.Application.Repositories;

public interface IGetCarsRepository
{
    Task<PagedResult<CarListItemModel>> GetCarsAsync(GetCarsQuery query, CancellationToken ct);
}