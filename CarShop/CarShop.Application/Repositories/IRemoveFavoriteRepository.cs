namespace CarShop.Application.Repositories;

public interface IRemoveFavoriteRepository
{
    Task<bool> RemoveFavoriteAsync(string userId, int carId, CancellationToken cancellationToken);
}
