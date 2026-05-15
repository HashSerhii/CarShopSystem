using Microsoft.Extensions.DependencyInjection;
using CarShop.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using CarShop.Application.Repositories;
using CarShop.Infrastructure.Repositories;
using CarShop.Application;
using CarShop.Application.Mediator;
using CarShop.Application.Mediator.Interfaces;
using CarShop.Infrastructure.Authentication;
using CarShop.Application.Services;

namespace CarShop.Infrastructure;

public static class InfrastructureExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("CarShopDbConnection")));
        
        services.AddIdentity<User, IdentityRole>()
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();
        
        services.AddJwtAuthentication(configuration);
        
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
        services.AddScoped<IGetCarsRepository, GetCarsRepository>();
        services.AddScoped<ICarPhotoRepository, CarPhotoRepository>();
        services.AddScoped<IAddFavoriteRepository, FavoritesRepository>();
        services.AddScoped<IGetFavoritesRepository, FavoritesRepository>();
        services.AddScoped<IRemoveFavoriteRepository, FavoritesRepository>();

        services.AddSingleton<IMediator, Mediator>();
        services.AddHttpContextAccessor();
        services.AddScoped<IUserContext, UserContext>();

        return services;
    }
}