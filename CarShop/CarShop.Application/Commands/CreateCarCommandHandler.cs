using CarShop.Application.Mediator.Interfaces;
using CarShop.Domain;
using CarShop.Application.Repositories;
using CarShop.Application.Services;

namespace CarShop.Application.Commands;

public sealed record CreateCarCommandHandler: ICommandHandler<CreateCarCommand,int>
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
        var car = new Car
        {
            BrandId = command.BrandId,
            Model = command.Model,
            Year = command.Year,
            Description = command.Description,
            Price = command.Price,
            OwnerId = userId
        };

        var newId = await _repository.AddCarAsync(car, cancellationToken);
        return newId;
    }
}