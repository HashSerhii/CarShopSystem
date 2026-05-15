using CarShop.Application.Mediator.Interfaces;
using CarShop.Application.Repositories;
using CarShop.Application.Services;

namespace CarShop.Application.Commands;

public sealed record RemoveFavoriteCommand(int CarId);

public sealed class RemoveFavoriteCommandHandler(
    IRemoveFavoriteRepository repository,
    IUserContext userContext) : ICommandHandler<RemoveFavoriteCommand, bool>
{
    public async Task<bool> ExecuteAsync(RemoveFavoriteCommand command, CancellationToken cancellationToken)
    {
        var userId = userContext.GetCurrentUserId()
                     ?? throw new UnauthorizedAccessException("User ID not found in token");

        return await repository.RemoveFavoriteAsync(userId, command.CarId, cancellationToken);
    }
}
