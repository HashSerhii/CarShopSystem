using CarShop.Application.Mediator.Interfaces;
using CarShop.Application.Repositories;
using CarShop.Application.Services;
using Microsoft.Extensions.Hosting;

namespace CarShop.Application.Commands;

public sealed class UploadCarPhotoCommandHandler : ICommandHandler<UploadCarPhotoCommand>
{
    private const string RelativePhotoPath = "/images/cars";

    private readonly IGetCarsRepository _carsRepository;
    private readonly ICarPhotoRepository _carPhotoRepository;
    private readonly IUserContext _userContext;
    private readonly IHostEnvironment _hostEnvironment;

    public UploadCarPhotoCommandHandler(
        IGetCarsRepository carsRepository,
        ICarPhotoRepository carPhotoRepository,
        IUserContext userContext,
        IHostEnvironment hostEnvironment)
    {
        _carsRepository = carsRepository;
        _carPhotoRepository = carPhotoRepository;
        _userContext = userContext;
        _hostEnvironment = hostEnvironment;
    }

    public async Task ExecuteAsync(UploadCarPhotoCommand command, CancellationToken cancellationToken)
    {
        var userId = _userContext.GetCurrentUserId();
        if (string.IsNullOrEmpty(userId))
        {
            throw new UnauthorizedAccessException("User ID not found in token");
        }

        var ownerId = await _carsRepository.GetCarOwnerIdAsync(command.CarId, cancellationToken);
        if (ownerId is null)
        {
            throw new KeyNotFoundException($"Car with ID {command.CarId} was not found");
        }

        if (ownerId != userId)
        {
            throw new UnauthorizedAccessException("You can upload photos only for your own cars");
        }

        if (command.Files.Count == 0)
        {
            return;
        }

        var uploadDirectory = Path.Combine(_hostEnvironment.ContentRootPath, "wwwroot", "images", "cars");
        Directory.CreateDirectory(uploadDirectory);

        var relativeUrls = new List<string>(command.Files.Count);

        foreach (var file in command.Files)
        {
            var extension = Path.GetExtension(file.FileName);
            var fileName = $"{Guid.NewGuid()}{extension}";
            var physicalPath = Path.Combine(uploadDirectory, fileName);

            await using (var stream = File.Create(physicalPath))
            {
                await file.CopyToAsync(stream, cancellationToken);
            }

            relativeUrls.Add($"{RelativePhotoPath}/{fileName}");
        }

        await _carPhotoRepository.AddPhotosAsync(command.CarId, relativeUrls, cancellationToken);
    }
}
