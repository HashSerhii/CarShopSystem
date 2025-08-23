using System.Threading;
using System.Threading.Tasks;

namespace CarShop.Application.Repositories;

public interface IAddFavoriteRepository
{
	Task AddFavoriteAsync(string userId, int carId, CancellationToken ct);
} 