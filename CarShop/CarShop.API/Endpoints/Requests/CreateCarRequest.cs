using CarShop.Application.Commands;

namespace CarShop.API.Endpoints.Requests;

public sealed record CreateCarRequest(
    int BrandId,
    string Model,
    int Year,
    int Mileage,
    string Description,
    decimal Price
)
{
    public CreateCarCommand ToCommand() =>
        new(BrandId, Model, Year, Mileage, Description, Price);
}