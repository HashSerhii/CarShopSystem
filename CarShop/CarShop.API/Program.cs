using CarShop.API.Endpoints;
using CarShop.Application;
using Microsoft.EntityFrameworkCore;
using CarShop.Infrastructure;
using Microsoft.AspNetCore.Identity;
using CarShop.Domain;
using CarShop.Application.Models;
using CarShop.ApplicationExtensions;


var builder = WebApplication.CreateBuilder(args);


builder.Services.AddInfrastructure(builder.Configuration);

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

app.MapUserEndpoints();

app.Run();