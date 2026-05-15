using CarShop.Domain;

namespace CarShop.Application.Queries;

public sealed record GetCarsQuery(
    int? BrandId,
    int? YearFrom,
    int? YearTo,
    decimal? PriceFrom,
    decimal? PriceTo,
    int? MileageFrom,
    int? MileageTo,
    string? Model,
    string? OwnerId,
    ListingStatus? Status,
    bool OnlyApproved = true,
    int Page = 1,
    int PageSize = 20,
    string? Sort = "price_asc"
);