using CarShop.Application.Constants;
using CarShop.Application.Mediator.Interfaces;
using CarShop.Application.Repositories;
using CarShop.Application.Services;

namespace CarShop.Application.Commands;

public sealed record DeleteCarCommandHandler : ICommandHandler<DeleteCarCommand,bool>
{
    private readonly IGetCarsRepository _repository;
    private readonly IUserContext _userContext;

    public DeleteCarCommandHandler(IGetCarsRepository repository, IUserContext userContext)
    {
        _repository = repository;
        _userContext = userContext;
    }

    public async Task<bool> ExecuteAsync(DeleteCarCommand command, CancellationToken cancellationToken)
    {
        var userId = _userContext.GetCurrentUserId();
        if (string.IsNullOrEmpty(userId))
        {
            throw new UnauthorizedAccessException("User ID not found in token");
        }

        var ownerId = await _repository.GetCarOwnerIdAsync(command.Id, cancellationToken);
        if (ownerId is null)
        {
            return false;
        }

        if (ownerId != userId && !_userContext.IsInRole(Roles.Admin))
        {
            throw new UnauthorizedAccessException("You are not allowed to delete this car");
        }

        return await _repository.DeleteCarAsync(command.Id, cancellationToken);
    }
}