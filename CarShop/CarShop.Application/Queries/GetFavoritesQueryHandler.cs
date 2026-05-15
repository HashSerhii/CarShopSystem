using CarShop.Application.DTOs;
using CarShop.Application.Mediator.Interfaces;
using CarShop.Application.Repositories;
using CarShop.Application.Services;

namespace CarShop.Application.Queries;

public sealed class GetFavoritesQueryHandler(
    IGetFavoritesRepository repository,
    IUserContext userContext) : IQueryHandler<GetFavoritesQuery, PagedResult<FavoriteModel>>
{
    public async Task<PagedResult<FavoriteModel>> ExecuteAsync(
        GetFavoritesQuery query,
        CancellationToken cancellationToken)
    {
        var userId = userContext.GetCurrentUserId()
                     ?? throw new UnauthorizedAccessException("User ID not found in token");

        return await repository.GetFavoritesAsync(userId, query.Page, query.PageSize, cancellationToken);
    }
}
