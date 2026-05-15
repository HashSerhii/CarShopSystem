using CarShop.Application.Constants;
using CarShop.Application.DTOs;
using CarShop.Application.Mediator.Interfaces;
using CarShop.Application.Repositories;
using CarShop.Application.Services;
using CarShop.Domain;

namespace CarShop.Application.Commands;

public sealed record CreateBrandCommand(string Name);

public sealed class CreateBrandCommandHandler(
    IGetBrandsRepository repository,
    IUserContext userContext) : ICommandHandler<CreateBrandCommand, BrandModel>
{
    public async Task<BrandModel> ExecuteAsync(CreateBrandCommand command, CancellationToken cancellationToken)
    {
        if (!userContext.IsInRole(Roles.Admin))
        {
            throw new UnauthorizedAccessException("Only administrators can add brands.");
        }

        var name = command.Name.Trim();
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Brand name is required.");
        }

        return await repository.AddBrandAsync(name, cancellationToken);
    }
}
