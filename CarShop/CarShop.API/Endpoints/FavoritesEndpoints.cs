using CarShop.Application.Commands;
using CarShop.Application.DTOs;
using CarShop.Application.Mediator.Interfaces;
using CarShop.Application.Queries;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace CarShop.API.Endpoints;

public static class FavoritesEndpoints
{
    public static IEndpointRouteBuilder MapFavoritesEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/favorites", async (AddFavoriteCommand command, ICommandHandler<AddFavoriteCommand> handler, CancellationToken ct) =>
        {
            await handler.ExecuteAsync(command, ct);
            return Results.Ok();
        })
        .WithSummary("Add car to favorites");

        app.MapGet("/favorites", async (string userId, int page, int pageSize, IQueryHandler<GetFavoritesQuery, PagedResult<FavoriteDto>> handler, CancellationToken ct) =>
        {
            var normalizedPage = page <= 0 ? 1 : page;
            var normalizedPageSize = pageSize is <= 0 or > 100 ? 20 : pageSize;
            var result = await handler.ExecuteAsync(new GetFavoritesQuery(userId, normalizedPage, normalizedPageSize), ct);
            return Results.Ok(result);
        })
        .WithSummary("Get user favorites");

        return app;
    }
} 