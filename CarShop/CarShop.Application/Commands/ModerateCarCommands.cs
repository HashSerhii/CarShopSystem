using CarShop.Application.Constants;
using CarShop.Application.Mediator.Interfaces;
using CarShop.Application.Repositories;
using CarShop.Application.Services;
using CarShop.Domain;

namespace CarShop.Application.Commands;

public sealed record ApproveCarCommand(int CarId);

public sealed class ApproveCarCommandHandler(
    IGetCarsRepository repository,
    IUserContext userContext) : ICommandHandler<ApproveCarCommand, bool>
{
    public async Task<bool> ExecuteAsync(ApproveCarCommand command, CancellationToken cancellationToken)
    {
        if (!userContext.IsInRole(Roles.Admin))
        {
            throw new UnauthorizedAccessException("Only administrators can approve listings.");
        }

        return await repository.UpdateCarStatusAsync(command.CarId, ListingStatus.Approved, cancellationToken);
    }
}

public sealed record RejectCarCommand(int CarId);

public sealed class RejectCarCommandHandler(
    IGetCarsRepository repository,
    IUserContext userContext) : ICommandHandler<RejectCarCommand, bool>
{
    public async Task<bool> ExecuteAsync(RejectCarCommand command, CancellationToken cancellationToken)
    {
        if (!userContext.IsInRole(Roles.Admin))
        {
            throw new UnauthorizedAccessException("Only administrators can reject listings.");
        }

        return await repository.UpdateCarStatusAsync(command.CarId, ListingStatus.Rejected, cancellationToken);
    }
}
