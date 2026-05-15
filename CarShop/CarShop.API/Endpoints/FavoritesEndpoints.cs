using CarShop.Application.Commands;
using CarShop.Application.DTOs;
using CarShop.Application.Mediator.Interfaces;
using CarShop.Application.Queries;
using CarShop.API.Endpoints.Requests;

namespace CarShop.API.Endpoints;

public static class FavoritesEndpoints
{
    public static IEndpointRouteBuilder MapFavoritesEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost(ApiRoutes.Favorites.Base, async (
                AddFavoriteRequest request,
                ICommandHandler<AddFavoriteCommand> handler,
                CancellationToken ct) =>
            {
                await handler.ExecuteAsync(new AddFavoriteCommand(request.CarId), ct);
                return Results.Ok();
            })
            .RequireAuthorization()
            .WithName("AddFavorite")
            .WithSummary("Add car to favorites");

        app.MapGet(ApiRoutes.Favorites.Base, async (
                [AsParameters] GetFavoritesRequest request,
                IQueryHandler<GetFavoritesQuery, PagedResult<FavoriteModel>> handler,
                CancellationToken ct) =>
            {
                var result = await handler.ExecuteAsync(request.ToQuery(), ct);
                return Results.Ok(result);
            })
            .RequireAuthorization()
            .WithName("GetFavorites")
            .WithSummary("Get user favorites");

        app.MapDelete(ApiRoutes.Favorites.ByCarId, async (
                int carId,
                ICommandHandler<RemoveFavoriteCommand, bool> handler,
                CancellationToken ct) =>
            {
                var removed = await handler.ExecuteAsync(new RemoveFavoriteCommand(carId), ct);
                return removed ? Results.NoContent() : Results.NotFound();
            })
            .RequireAuthorization()
            .WithName("RemoveFavorite")
            .WithSummary("Remove car from favorites");

        return app;
    }
}
