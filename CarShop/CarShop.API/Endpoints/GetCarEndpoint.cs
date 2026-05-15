using CarShop.Application.Constants;
using CarShop.Application.DTOs;              
using CarShop.Application.Mediator.Interfaces; 
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using CarShop.API.Endpoints.Requests;
using CarShop.API.Endpoints;
using CarShop.Application.Commands;
using CarShop.Application.Queries;
using CarShop.Application.Services;
using CarShop.Domain;

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

        app.MapGet(ApiRoutes.Cars.Pending, async (
                int? page,
                int? pageSize,
                IQueryHandler<GetCarsQuery, PagedResult<CarListItemModel>> handler,
                IUserContext userContext,
                CancellationToken ct) =>
            {
                if (!userContext.IsInRole(Roles.Admin))
                {
                    return Results.Forbid();
                }

                var resolvedPage = page is null or <= 0 ? 1 : page.Value;
                var resolvedPageSize = pageSize is null or <= 0 ? 20 : Math.Min(pageSize.Value, 100);

                var query = new GetCarsQuery(
                    null, null, null, null, null, null, null, null, null,
                    ListingStatus.Pending,
                    OnlyApproved: false,
                    resolvedPage,
                    resolvedPageSize);

                var result = await handler.ExecuteAsync(query, ct);
                return Results.Ok(result);
            })
            .RequireAuthorization()
            .WithName("GetPendingCars")
            .WithSummary("Returns listings awaiting admin approval");

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
                CreateCarRequest request,
                IMediator mediator,
                CancellationToken ct) =>
            {
                var command = request.ToCommand();
                var newId = await mediator.ExecuteCommand<CreateCarCommand, int>(command, ct);

                return Results.Created(
                    $"{ApiRoutes.Cars.Base}/{newId}",
                    new CreateCarResponse(newId));
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

        app.MapPost(ApiRoutes.Cars.Approve, async (
                int id,
                ICommandHandler<ApproveCarCommand, bool> handler,
                CancellationToken ct) =>
            {
                var ok = await handler.ExecuteAsync(new ApproveCarCommand(id), ct);
                return ok ? Results.NoContent() : Results.NotFound();
            })
            .RequireAuthorization()
            .WithName("ApproveCar")
            .WithSummary("Approve a pending listing");

        app.MapPost(ApiRoutes.Cars.Reject, async (
                int id,
                ICommandHandler<RejectCarCommand, bool> handler,
                CancellationToken ct) =>
            {
                var ok = await handler.ExecuteAsync(new RejectCarCommand(id), ct);
                return ok ? Results.NoContent() : Results.NotFound();
            })
            .RequireAuthorization()
            .WithName("RejectCar")
            .WithSummary("Reject a pending listing");

    return app;
    }
}