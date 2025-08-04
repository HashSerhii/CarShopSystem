using CarShop.Application.Models;
using Microsoft.AspNetCore.Identity;
namespace CarShop.Application;

public interface IUserService
{
    Task<IdentityResult> RegisterAsync(RegisterModel model);
    
    Task<bool> LoginAsync(LoginModel model);
}
