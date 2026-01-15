using System.Windows.Input;
using CarShop.Application.Mediator.Interfaces;

namespace CarShop.Application.Commands;

public sealed record CreateCarCommand(
    int BrandId,
    string Model,
    int Year,
    string Description,
    decimal Price
    );