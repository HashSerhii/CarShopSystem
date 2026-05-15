using Microsoft.AspNetCore.Http;

namespace CarShop.Application.Commands;

public sealed record UploadCarPhotoCommand(int CarId, IFormFileCollection Files);
