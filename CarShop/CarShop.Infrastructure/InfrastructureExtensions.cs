using Microsoft.Extensions.DependencyInjection;
using CarShop.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using CarShop.Application.Repositories;
using CarShop.Infrastructure.Repositories;
using CarShop.Application;
using CarShop.Infrastructure.Authentication;

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
        
        return services;
    }
}