using CarShop.Application.Commands;
using CarShop.Application.DTOs;
using CarShop.Application;
using CarShop.Application.Mediator.Interfaces;
using CarShop.Application.Queries;
using CarShop.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace CarShop.ApplicationExtensions;

public static class ApplicationExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();

        services.AddScoped<IQueryHandler<GetCarsQuery, PagedResult<CarListItemModel>>, GetCarsQueryHandler>();
        services.AddScoped<IQueryHandler<GetFavoritesQuery, PagedResult<FavoriteModel>>, GetFavoritesQueryHandler>();
        services.AddScoped<IQueryHandler<GetCarByIdQuery, CarDetailModel?>, GetCarByIdQueryHandler>();
        services.AddScoped<IQueryHandler<GetBrandsQuery, IReadOnlyList<BrandModel>>, GetBrandsQueryHandler>();
        services.AddScoped<ICommandHandler<AddFavoriteCommand>, AddFavoriteCommandHandler>();
        services.AddScoped<ICommandHandler<CreateCarCommand, int>, CreateCarCommandHandler>();
        services.AddScoped<ICommandHandler<DeleteCarCommand, bool>, DeleteCarCommandHandler>();
        services.AddScoped<ICommandHandler<UploadCarPhotoCommand>, UploadCarPhotoCommandHandler>();
        services.AddScoped<ICommandHandler<RemoveFavoriteCommand, bool>, RemoveFavoriteCommandHandler>();

        return services;
    }
}