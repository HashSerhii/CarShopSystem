namespace CarShop.Application.DTOs;

public sealed record CarListItemModel(
    int Id,
    string Brand,
    string Model,
    int Year,
    decimal Price,
    int Mileage,
    string Status,
    string? PrimaryPhotoUrl
);

public sealed record PagedResult<T>(
    IReadOnlyList<T> Items, int Total, int Page, int PageSize
); 