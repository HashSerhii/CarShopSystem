using CarShop.Application.DTOs;              
using CarShop.Application.Mediator.Interfaces; 
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using CarShop.API.Endpoints.Requests;
using CarShop.API.Endpoints;
using CarShop.Application.Queries;

namespace CarShop.API.Endpoints;

public static class CarEndpoints
{
    public static IEndpointRouteBuilder MapCarEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet(ApiRoutes.Cars, async (
                [AsParameters] GetCarsRequest request,
                IQueryHandler<GetCarsQuery, PagedResult<CarListItemModel>> handler,
                CancellationToken ct) =>
            {
                var result = await handler.ExecuteAsync(request.ToQuery(), ct);
                return Results.Ok(result);
            })
            .WithName("GetCars")
            .WithSummary("Returns a list of cars with filters, sorting")
            .Produces<PagedResult<CarListItemModel>>(StatusCodes.Status200OK);

        return app;
    }
}