using CarShop.API;
using CarShop.API.Endpoints;
using CarShop.Infrastructure;
using CarShop.ApplicationExtensions;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);


builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddApplicationServices();
builder.Services.AddFrontendCors(builder.Configuration);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerDocumentation();
builder.Services.AddProblemDetails();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await db.Database.MigrateAsync();
}

await DataSeeder.SeedAsync(app.Services);
await IdentityDbSeeder.SeedAsync(app.Services);

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseExceptionHandler();
}

app.UseHttpsRedirection();
app.UseFrontendCors();
app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();

app.MapUserEndpoints();
app.MapBrandsEndpoints();
app.MapCarEndpoints();
app.MapFavoritesEndpoints();

app.Run();