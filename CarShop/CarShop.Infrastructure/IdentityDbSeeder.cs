using CarShop.Application.Constants;
using CarShop.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace CarShop.Infrastructure;

public static class IdentityDbSeeder
{
    private const string DefaultAdminEmail = "admin@carshop.com";
    private const string DefaultAdminPassword = "Admin123!";

    public static async Task SeedAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();

        foreach (var roleName in new[] { Roles.Admin, Roles.User })
        {
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                await roleManager.CreateAsync(new IdentityRole(roleName));
            }
        }

        var admin = await userManager.FindByEmailAsync(DefaultAdminEmail);
        if (admin is not null)
        {
            if (string.IsNullOrWhiteSpace(admin.PhoneNumber))
            {
                admin.PhoneNumber = "+380501112233";
                await userManager.UpdateAsync(admin);
            }

            return;
        }

        admin = new User
        {
            UserName = DefaultAdminEmail,
            Email = DefaultAdminEmail,
            EmailConfirmed = true,
            PhoneNumber = "+380501112233"
        };

        var createResult = await userManager.CreateAsync(admin, DefaultAdminPassword);
        if (!createResult.Succeeded)
        {
            throw new InvalidOperationException(
                $"Failed to create default admin: {string.Join(", ", createResult.Errors.Select(e => e.Description))}");
        }

        await userManager.AddToRoleAsync(admin, Roles.Admin);
        await userManager.AddToRoleAsync(admin, Roles.User);
    }
}
