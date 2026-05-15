using CarShop.Application.Commands;
using CarShop.Application.DTOs;
using CarShop.Application.Mediator.Interfaces;
using CarShop.Application.Queries;

namespace CarShop.API.Endpoints;

public sealed record CreateBrandRequest(string Name);

public static class BrandsEndpoints
{
    public static IEndpointRouteBuilder MapBrandsEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet(ApiRoutes.Brands.Base, async (
                IQueryHandler<GetBrandsQuery, IReadOnlyList<BrandModel>> handler,
                CancellationToken ct) =>
            {
                var brands = await handler.ExecuteAsync(new GetBrandsQuery(), ct);
                return Results.Ok(brands);
            })
            .WithName("GetBrands")
            .WithSummary("Get all car brands");

        app.MapPost(ApiRoutes.Brands.Base, async (
                CreateBrandRequest request,
                ICommandHandler<CreateBrandCommand, BrandModel> handler,
                CancellationToken ct) =>
            {
                var brand = await handler.ExecuteAsync(new CreateBrandCommand(request.Name), ct);
                return Results.Created($"{ApiRoutes.Brands.Base}/{brand.Id}", brand);
            })
            .RequireAuthorization()
            .WithName("CreateBrand")
            .WithSummary("Add a new car brand (admin only)");

        return app;
    }
}
