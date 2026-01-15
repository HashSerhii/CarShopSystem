using CarShop.Application.Commands;

namespace CarShop.API.Endpoints.Requests;

public sealed record DeleteCarRequest(int Id)
{
    public DeleteCarCommand ToCommand() => new DeleteCarCommand(Id);
}