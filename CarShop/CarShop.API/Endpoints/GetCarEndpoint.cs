using CarShop.Application.DTOs;              
using CarShop.Application.Mediator.Interfaces; 
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using CarShop.API.Endpoints.Requests;
using CarShop.API.Endpoints;
using CarShop.Application.Commands;
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

        app.MapGet("/api/cars/{id:int}", async (
                [AsParameters] GetCarByIdRequest request,
                IMediator mediator,
                CancellationToken ct) =>
            {
                var query = request.ToQuery();
                var result = await mediator.ExecuteQuery<GetCarByIdQuery, CarDetailModel?>(query, ct);

                return result is not null
                    ? Results.Ok(result)
                    : Results.NotFound();
            })
            .WithName("GetCarById")
            .WithSummary("Get car details by ID");

        app.MapPost("/api/cars", async (
                [AsParameters] CreateCarRequest request,
                IMediator mediator,
                CancellationToken ct) =>
            {
                var command = request.ToCommand();
                var newId = await mediator.ExecuteCommand<CreateCarCommand, int>(command, ct);

                return Results.Created($"/api/cars/{newId}", newId);
            })
            .WithName("CreateCar")
            .WithSummary("Create a new car");

        app.MapDelete("/api/cars/{id:int}", async (
                [AsParameters] DeleteCarRequest request,
                IMediator mediator,
                CancellationToken ct) =>
            {
                var command = request.ToCommand();

                var succes = await mediator.ExecuteCommand<DeleteCarCommand, bool>(command, ct);

                return succes
                    ? Results.NoContent()
                    : Results.NotFound();
            })
            .WithName("DeleteCar")
            .WithSummary("Deletes a car by ID");

    return app;
    }
}