using CarShop.Application.DTOs;

namespace CarShop.Application.Repositories;

public interface IGetBrandsRepository
{
    Task<IReadOnlyList<BrandModel>> GetBrandsAsync(CancellationToken cancellationToken);
}
