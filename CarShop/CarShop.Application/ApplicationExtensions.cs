using CarShop.Application.Services;
using CarShop.Application;
using CarShop.Application.DTOs;
using CarShop.Application.Mediator.Interfaces;
using CarShop.Application.Queries;
using CarShop.Application.Repositories;
using Microsoft.Extensions.DependencyInjection;


namespace CarShop.ApplicationExtensions
{
    public static class ApplicationExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();

            services.AddScoped<IQueryHandler<GetCarsQuery, PagedResult<CarListItemModel>>, GetCarsQueryHandler>();
            
            return services;
        }
    }
}