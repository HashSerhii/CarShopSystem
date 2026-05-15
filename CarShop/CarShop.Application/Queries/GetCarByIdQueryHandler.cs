using CarShop.Application.Constants;
using CarShop.Application.DTOs;
using CarShop.Application.Mediator.Interfaces;
using CarShop.Application.Repositories;
using CarShop.Application.Services;
using CarShop.Domain;

namespace CarShop.Application.Queries;

public sealed class GetCarByIdQueryHandler : IQueryHandler<GetCarByIdQuery, CarDetailModel?>
{
    private readonly IGetCarsRepository _carsRepository;
    private readonly IUserContext _userContext;

    public GetCarByIdQueryHandler(IGetCarsRepository carsRepository, IUserContext userContext)
    {
        _carsRepository = carsRepository;
        _userContext = userContext;
    }

    public async Task<CarDetailModel?> ExecuteAsync(
        GetCarByIdQuery query,
        CancellationToken cancellationToken)
    {
        var car = await _carsRepository.GetCarByIdAsync(query.CarId, cancellationToken);
        if (car is null)
        {
            return null;
        }

        if (string.Equals(car.Status, nameof(ListingStatus.Approved), StringComparison.Ordinal))
        {
            return car;
        }

        var userId = _userContext.GetCurrentUserId();
        var ownerId = await _carsRepository.GetCarOwnerIdAsync(query.CarId, cancellationToken);

        if (_userContext.IsInRole(Roles.Admin) || (!string.IsNullOrEmpty(userId) && userId == ownerId))
        {
            return car;
        }

        return null;
    }
}
