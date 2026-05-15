using CarShop.Application;
using CarShop.Application.Models;

namespace CarShop.API.Endpoints;

public static class UserEndpoints
{
    public static IEndpointRouteBuilder MapUserEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost(ApiRoutes.Users.Register, async (RegisterDto dto, IUserService userService) =>
        {
            var result = await userService.RegisterAsync(dto);
            return result.Succeeded
                ? Results.Ok("Registration was successful")
                : Results.BadRequest(result.Errors);
        })
        .WithName("Register")
        .WithOpenApi();

        app.MapPost(ApiRoutes.Users.Login, async (LoginDto dto, IUserService userService) =>
        {
            var authResult = await userService.LoginAsync(dto);
            return authResult is not null
                ? Results.Ok(authResult)
                : Results.BadRequest("Incorrect email or password");
        })
        .WithName("Login")
        .WithOpenApi();

        return app;
    }
}