using CarShop.Application.Commands;
using CarShop.Application.DTOs;
using CarShop.Application.Mediator.Interfaces;
using CarShop.Application.Queries;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CarShop.API.Endpoints.Requests;

namespace CarShop.API.Endpoints;

public static class FavoritesEndpoints
{
    public static IEndpointRouteBuilder MapFavoritesEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost(ApiRoutes.Favorites.Base, async (AddFavoriteCommand command, ICommandHandler<AddFavoriteCommand> handler, CancellationToken ct) =>
        {
            await handler.ExecuteAsync(command, ct);
            return Results.Ok();
        })
        .WithSummary("Add car to favorites");

        app.MapGet(ApiRoutes.Favorites.Base, async ([AsParameters] GetFavoritesRequest request, IQueryHandler<GetFavoritesQuery, PagedResult<FavoriteModel>> handler, CancellationToken ct) =>
        {
            var result = await handler.ExecuteAsync(request.ToQuery(), ct);
            return Results.Ok(result);
        })
        .WithSummary("Get user favorites");

        app.MapDelete(ApiRoutes.Favorites.ByCarId, async (
                int carId,
                [FromBody] RemoveFavoriteRequest request,
                ICommandHandler<RemoveFavoriteCommand, bool> handler,
                CancellationToken ct) =>
            {
                var removed = await handler.ExecuteAsync(
                    new RemoveFavoriteCommand(request.UserId, carId),
                    ct);

                return removed
                    ? Results.NoContent()
                    : Results.NotFound();
            })
            .WithSummary("Remove car from favorites");

        return app;
    }
} 