using CarShop.Application.DTOs;
using CarShop.Application.Mediator.Interfaces;
using CarShop.Application.Repositories;

namespace CarShop.Application.Queries;

public sealed class GetCarByIdQueryHandler
 : IQueryHandler<GetCarByIdQuery, CarDetailModel?>
{
    private readonly IGetCarsRepository _carsRepository;

    public GetCarByIdQueryHandler(IGetCarsRepository carsRepository)
    {
        _carsRepository = carsRepository;
    }

    public async Task<CarDetailModel?> ExecuteAsync(
        GetCarByIdQuery query,
        CancellationToken cancellationToken)
    {
        return await _carsRepository.GetCarByIdAsync(query.CarId, cancellationToken);
    }
}