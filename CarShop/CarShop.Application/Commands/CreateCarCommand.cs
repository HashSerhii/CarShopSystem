namespace CarShop.Application.Commands;

public sealed record CreateCarCommand(
    int BrandId,
    string Model,
    int Year,
    int Mileage,
    string Description,
    decimal Price
    );