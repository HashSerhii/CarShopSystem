using CarShop.Application.Constants;
using CarShop.Application.Queries;

namespace CarShop.API.Endpoints.Requests;

public sealed record GetCarsRequest(
    int? BrandId,
    int? YearFrom,
    int? YearTo,
    decimal? PriceFrom,
    decimal? PriceTo,
    int? MileageFrom,
    int? MileageTo,
    string? Model,
    bool Mine = false,
    int Page = 1,
    int PageSize = 20,
    string? Sort = CarSort.PriceAsc)
{
    public GetCarsQuery ToQuery(string? ownerId = null) => new(
        BrandId,
        YearFrom,
        YearTo,
        PriceFrom,
        PriceTo,
        MileageFrom,
        MileageTo,
        Model,
        ownerId,
        Status: null,
        OnlyApproved: ownerId is null,
        Math.Clamp(Page, 1, int.MaxValue),
        Math.Clamp(PageSize, 1, 100),
        CarSort.Normalize(Sort)
    );
} 