namespace CarShop.Application.DTOs;

public sealed record FavoriteDto(
    int CarId,
    string Brand,
    string Model,
    int Year,
    decimal Price,
    string? MainPhotoUrl
    );