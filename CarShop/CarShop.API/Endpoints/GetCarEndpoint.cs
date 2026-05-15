using CarShop.Application.DTOs;              
using CarShop.Application.Mediator.Interfaces; 
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using CarShop.API.Endpoints.Requests;
using CarShop.API.Endpoints;
using CarShop.Application.Commands;
using CarShop.Application.Queries;
using CarShop.Application.Services;

namespace CarShop.API.Endpoints;

public static class CarEndpoints
{
    public static IEndpointRouteBuilder MapCarEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet(ApiRoutes.Cars.Base, async (
                [AsParameters] GetCarsRequest request,
                IQueryHandler<GetCarsQuery, PagedResult<CarListItemModel>> handler,
                IUserContext userContext,
                CancellationToken ct) =>
            {
                string? ownerId = request.Mine ? userContext.GetCurrentUserId() : null;
                if (request.Mine && string.IsNullOrEmpty(ownerId))
                {
                    return Results.Unauthorized();
                }

                var result = await handler.ExecuteAsync(request.ToQuery(ownerId), ct);
                return Results.Ok(result);
            })
            .WithName("GetCars")
            .WithSummary("Returns a list of cars with filters, sorting")
            .Produces<PagedResult<CarListItemModel>>(StatusCodes.Status200OK);

        app.MapGet($"{ApiRoutes.Cars.Base}/mine", async (
                [AsParameters] GetCarsRequest request,
                IQueryHandler<GetCarsQuery, PagedResult<CarListItemModel>> handler,
                IUserContext userContext,
                CancellationToken ct) =>
            {
                var ownerId = userContext.GetCurrentUserId();
                if (string.IsNullOrEmpty(ownerId))
                {
                    return Results.Unauthorized();
                }

                var result = await handler.ExecuteAsync(request.ToQuery(ownerId), ct);
                return Results.Ok(result);
            })
            .RequireAuthorization()
            .WithName("GetMyCars")
            .WithSummary("Returns cars listed by the current user");

        app.MapGet(ApiRoutes.Cars.ById, async (
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

        app.MapPost(ApiRoutes.Cars.Base, async (
                [AsParameters] CreateCarRequest request,
                IMediator mediator,
                CancellationToken ct) =>
            {
                var command = request.ToCommand();
                var newId = await mediator.ExecuteCommand<CreateCarCommand, int>(command, ct);

                return Results.Created($"{ApiRoutes.Cars.Base}/{newId}", newId);
            })
            .RequireAuthorization()
            .WithName("CreateCar")
            .WithSummary("Create a new car");

        app.MapPost(ApiRoutes.Cars.Photos, async (
                int id,
                IFormFileCollection files,
                IMediator mediator,
                CancellationToken ct) =>
            {
                var command = new UploadCarPhotoCommand(id, files);
                await mediator.ExecuteCommand(command, ct);
                return Results.Ok();
            })
            .RequireAuthorization()
            .DisableAntiforgery()
            .WithName("UploadCarPhotos")
            .WithSummary("Upload photos for a car");

        app.MapDelete(ApiRoutes.Cars.ById, async (
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
            .RequireAuthorization()
            .WithName("DeleteCar")
            .WithSummary("Deletes a car by ID");

    return app;
    }
}