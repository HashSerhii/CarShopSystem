namespace CarShop.Application.Queries;

public sealed record GetCarsQuery(
    int? BrandId,
    int? YearFrom,
    int? YearTo,
    decimal? PriceFrom,
    decimal? PriceTo,
    string? OwnerId,
    int Page = 1,
    int PageSize = 20,
    string? Sort = "price_asc"
);