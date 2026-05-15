using CarShop.Application.DTOs;
using CarShop.Application.Mediator.Interfaces;
using CarShop.Application.Repositories;


namespace CarShop.Application.Queries;

public sealed class GetCarsQueryHandler(IGetCarsRepository carsRepository)
    : IQueryHandler<GetCarsQuery, PagedResult<CarListItemModel>>
{
    public Task<PagedResult<CarListItemModel>> ExecuteAsync(
        GetCarsQuery query,
        CancellationToken cancellationToken) =>
        carsRepository.GetCarsAsync(query, cancellationToken);
}