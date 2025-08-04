using CarShop.Application.Services;
using CarShop.Application;

using Microsoft.Extensions.DependencyInjection;

namespace CarShop.ApplicationExtensions
{
    public static class ApplicationExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            
            services.AddScoped<IUserService, UserService>();

            
            return services;
        }
    }
}