using CarShop.Application.Constants;
using CarShop.Application.Mediator.Interfaces;
using CarShop.Application.Repositories;
using CarShop.Application.Services;
using CarShop.Domain;

namespace CarShop.Application.Commands;

public sealed class CreateCarCommandHandler : ICommandHandler<CreateCarCommand, int>
{
    private readonly IGetCarsRepository _repository;
    private readonly IUserContext _userContext;

    public CreateCarCommandHandler(IGetCarsRepository repository, IUserContext userContext)
    {
        _repository = repository;
        _userContext = userContext;
    }

    public async Task<int> ExecuteAsync(CreateCarCommand command, CancellationToken cancellationToken)
    {
        var userId = _userContext.GetCurrentUserId();

        if (string.IsNullOrEmpty(userId))
        {
            throw new UnauthorizedAccessException("User ID not found in token");
        }

        var isAdmin = _userContext.IsInRole(Roles.Admin);
        var car = new Car
        {
            BrandId = command.BrandId,
            Model = command.Model.Trim(),
            Year = command.Year,
            Mileage = command.Mileage,
            Description = command.Description.Trim(),
            Price = command.Price,
            OwnerId = userId,
            Status = isAdmin ? ListingStatus.Approved : ListingStatus.Pending
        };

        return await _repository.AddCarAsync(car, cancellationToken);
    }
}
