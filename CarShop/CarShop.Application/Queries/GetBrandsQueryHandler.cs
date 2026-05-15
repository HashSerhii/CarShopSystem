using CarShop.Application.DTOs;
using CarShop.Application.Mediator.Interfaces;
using CarShop.Application.Repositories;

namespace CarShop.Application.Queries;

public sealed class GetBrandsQueryHandler(IGetBrandsRepository repository)
    : IQueryHandler<GetBrandsQuery, IReadOnlyList<BrandModel>>
{
    public Task<IReadOnlyList<BrandModel>> ExecuteAsync(GetBrandsQuery query, CancellationToken cancellationToken) =>
        repository.GetBrandsAsync(cancellationToken);
}
