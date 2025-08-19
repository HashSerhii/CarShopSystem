using CarShop.Application.DTOs;              
using CarShop.Application.Queries;          
using CarShop.Application.Mediator.Interfaces; 
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace CarShop.API.Endpoints;

public static class CarEndpoints
{
    public static IEndpointRouteBuilder MapCarEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/cars", async (
                [AsParameters] GetCarsQuery query,
                IQueryHandler<GetCarsQuery, PagedResult<CarListItemDto>> handler,
                CancellationToken ct) =>
            {
                
                var normalized = query with
                {
                    Page = query.Page <= 0 ? 1 : query.Page,
                    PageSize = query.PageSize is <= 0 or > 100 ? 20 : query.PageSize,
                    Sort = NormalizeSort(query.Sort)
                };

                var result = await handler.ExecuteAsync(normalized, ct);
                return Results.Ok(result);
            })
            .WithName("GetCars")
            .WithSummary("Returns a list of cars with filters, sorting")
            .Produces<PagedResult<CarListItemDto>>(StatusCodes.Status200OK);

        return app;
    }

    private static string NormalizeSort(string? sort) =>
        sort?.ToLowerInvariant() switch
        {
            "price_desc" => "price_desc",
            "year_asc"   => "year_asc",
            "year_desc"  => "year_desc",
            _            => "price_asc" 
        };
}