using CarShop.Application.DTOs;
using System.Threading;
using System.Threading.Tasks;

namespace CarShop.Application.Repositories;

public interface IGetFavoritesRepository
{
	Task<PagedResult<FavoriteModel>> GetFavoritesAsync(string userId, int page, int pageSize, CancellationToken ct);
} 