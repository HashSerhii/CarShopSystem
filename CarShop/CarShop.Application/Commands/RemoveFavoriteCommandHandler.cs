using CarShop.Application.Mediator.Interfaces;
using CarShop.Application.Repositories;

namespace CarShop.Application.Commands;

public sealed class RemoveFavoriteCommandHandler(IRemoveFavoriteRepository repository)
    : ICommandHandler<RemoveFavoriteCommand, bool>
{
    public Task<bool> ExecuteAsync(RemoveFavoriteCommand command, CancellationToken cancellationToken) =>
        repository.RemoveFavoriteAsync(command.UserId, command.CarId, cancellationToken);
}
