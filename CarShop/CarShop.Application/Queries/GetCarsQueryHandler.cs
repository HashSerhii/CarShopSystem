using CarShop.Application.DTOs;
using CarShop.Application.Mediator.Interfaces;
using CarShop.Application.Repositories;
using CarShop.Application.Services;

namespace CarShop.Application.Queries;

public sealed class GetCarsQueryHandler(
    IGetCarsRepository carsRepository,
    IUserContext userContext) : IQueryHandler<GetCarsQuery, PagedResult<CarListItemModel>>
{
    public async Task<PagedResult<CarListItemModel>> ExecuteAsync(
        GetCarsQuery query,
        CancellationToken cancellationToken)
    {
        if (query.OwnerId is not null &&
            string.IsNullOrEmpty(userContext.GetCurrentUserId()))
        {
            throw new UnauthorizedAccessException("Authentication required for personal listings");
        }

        return await carsRepository.GetCarsAsync(query, cancellationToken);
    }
}
