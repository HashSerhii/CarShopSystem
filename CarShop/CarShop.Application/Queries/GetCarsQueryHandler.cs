using CarShop.Application.DTOs;
using CarShop.Application.Mediator.Interfaces;
using CarShop.Application.Repositories;


namespace CarShop.Application.Queries;

public sealed class GetCarsQueryHandler(IGetCarsRepository carsRepository)
    : IQueryHandler<GetCarsQuery, PagedResult<CarListItemDto>>
{
    public Task<PagedResult<CarListItemDto>> ExecuteAsync(
        GetCarsQuery query,
        CancellationToken cancellationToken) =>
        carsRepository.GetCarsAsync(query, cancellationToken);
}