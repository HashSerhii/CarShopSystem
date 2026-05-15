using CarShop.API;
using CarShop.API.Endpoints;
using CarShop.Infrastructure;
using CarShop.ApplicationExtensions;


var builder = WebApplication.CreateBuilder(args);


builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddApplicationServices();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerDocumentation();

var app = builder.Build();

await IdentityDbSeeder.SeedAsync(app.Services);

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();

app.MapUserEndpoints();
app.MapCarEndpoints();
app.MapFavoritesEndpoints();

app.Run();