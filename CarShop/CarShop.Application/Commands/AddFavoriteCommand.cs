namespace CarShop.Application.Commands;

using CarShop.Application.Mediator.Interfaces;
using CarShop.Application.Repositories;

public sealed record AddFavoriteCommand(string UserId, int CarId);

public sealed class AddFavoriteCommandHandler(IAddFavoriteRepository repository)
	: ICommandHandler<AddFavoriteCommand>
{
	public Task ExecuteAsync(AddFavoriteCommand command, CancellationToken cancellationToken) =>
		repository.AddFavoriteAsync(command.UserId, command.CarId, cancellationToken);
}