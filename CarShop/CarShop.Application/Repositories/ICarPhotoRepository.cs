namespace CarShop.Application.Repositories;

public interface ICarPhotoRepository
{
    Task AddPhotosAsync(int carId, IReadOnlyList<string> urls, CancellationToken cancellationToken);
}
