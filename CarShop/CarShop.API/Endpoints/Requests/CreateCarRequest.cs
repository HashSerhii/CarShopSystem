using CarShop.Application.Commands;

namespace CarShop.API.Endpoints.Requests;

public sealed record CreateCarRequest(
    int BrandId,
    string Model,
    int Year,
    string Description,
    decimal Price
)
{
    public CreateCarCommand ToCommand() =>
        new CreateCarCommand(BrandId, Model, Year, Description, Price);
}