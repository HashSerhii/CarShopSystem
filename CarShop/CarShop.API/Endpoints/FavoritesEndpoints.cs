using CarShop.Application.Commands;
using CarShop.Application.DTOs;
using CarShop.Application.Mediator.Interfaces;
using CarShop.Application.Queries;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using CarShop.API.Endpoints.Requests;

namespace CarShop.API.Endpoints;

public static class FavoritesEndpoints
{
    public static IEndpointRouteBuilder MapFavoritesEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost(ApiRoutes.Favorites, async (AddFavoriteCommand command, ICommandHandler<AddFavoriteCommand> handler, CancellationToken ct) =>
        {
            await handler.ExecuteAsync(command, ct);
            return Results.Ok();
        })
        .WithSummary("Add car to favorites");

        app.MapGet(ApiRoutes.Favorites, async ([AsParameters] GetFavoritesRequest request, IQueryHandler<GetFavoritesQuery, PagedResult<FavoriteModel>> handler, CancellationToken ct) =>
        {
            var result = await handler.ExecuteAsync(request.ToQuery(), ct);
            return Results.Ok(result);
        })
        .WithSummary("Get user favorites");

        return app;
    }
} 