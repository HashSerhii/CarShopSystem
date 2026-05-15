using CarShop.Application.DTOs;
using CarShop.Application.Mediator.Interfaces;
using CarShop.Application.Queries;

namespace CarShop.API.Endpoints;

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

        return app;
    }
}
