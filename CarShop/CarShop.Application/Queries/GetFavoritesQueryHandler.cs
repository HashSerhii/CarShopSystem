using CarShop.Application.DTOs;
using CarShop.Application.Mediator.Interfaces;
using CarShop.Application.Repositories;

namespace CarShop.Application.Queries;

public sealed class GetFavoritesQueryHandler(IGetFavoritesRepository repository)
	: IQueryHandler<GetFavoritesQuery, PagedResult<FavoriteDto>>
{
	public Task<PagedResult<FavoriteDto>> ExecuteAsync(GetFavoritesQuery query, CancellationToken cancellationToken) =>
		repository.GetFavoritesAsync(query.UserId, query.Page, query.PageSize, cancellationToken);
} 