using CarShop.Application;
using Microsoft.EntityFrameworkCore;
using CarShop.Infrastructure;
using Microsoft.AspNetCore.Identity;
using CarShop.Domain;
using CarShop.Application.Models;
using CarShop.ApplicationExtensions;


var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();


builder.Services.AddApplicationServices();


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();


app.MapPost("/register", async (RegisterModel model, IUserService userService) =>
{
    var result = await userService.RegisterAsync(model);

    if (result.Succeeded)
        return Results.Ok("Registration was successful");

    return Results.BadRequest(result.Errors);
});


app.MapPost("/login", async (LoginModel model, IUserService userService) =>
{
    var result = await userService.LoginAsync(model);

    if (result)
        return Results.Ok("Login done");

    return Results.BadRequest("Incorrect email or password");
});

app.Run();