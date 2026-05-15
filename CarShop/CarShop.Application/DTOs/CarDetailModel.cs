namespace CarShop.Application.DTOs;

public sealed record CarDetailModel(
    int Id,
    string Brand,
    string Model,
    int Year,
    decimal Price,
    string? Description,
    string? MainPhotoUrl,
    List<string>? AllPhotoUrls,
    string? OwnerPhoneNumber
    );