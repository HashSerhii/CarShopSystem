using CarShop.Application.Repositories;
using CarShop.Domain;
using Microsoft.EntityFrameworkCore;

namespace CarShop.Infrastructure.Repositories;

public class CarPhotoRepository(AppDbContext db) : ICarPhotoRepository
{
    public async Task AddPhotosAsync(int carId, IReadOnlyList<string> urls, CancellationToken cancellationToken)
    {
        var photos = urls.Select(url => new CarPhoto
        {
            CarId = carId,
            Url = url
        });

        await db.CarPhotos.AddRangeAsync(photos, cancellationToken);
        await db.SaveChangesAsync(cancellationToken);
    }
}
