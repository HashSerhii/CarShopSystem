namespace CarShop.Application.DTOs;

public sealed record FavoriteModel(
    int CarId,
    string Brand,
    string Model,
    int Year,
    decimal Price,
    string? MainPhotoUrl
); 