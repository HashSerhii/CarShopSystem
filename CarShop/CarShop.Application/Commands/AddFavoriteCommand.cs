using CarShop.Application.Mediator.Interfaces;
using CarShop.Application.Repositories;
using CarShop.Application.Services;

namespace CarShop.Application.Commands;

public sealed record AddFavoriteCommand(int CarId);

public sealed class AddFavoriteCommandHandler(
    IAddFavoriteRepository repository,
    IUserContext userContext) : ICommandHandler<AddFavoriteCommand>
{
    public async Task ExecuteAsync(AddFavoriteCommand command, CancellationToken cancellationToken)
    {
        var userId = userContext.GetCurrentUserId()
                     ?? throw new UnauthorizedAccessException("User ID not found in token");

        await repository.AddFavoriteAsync(userId, command.CarId, cancellationToken);
    }
}
