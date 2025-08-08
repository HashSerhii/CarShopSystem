using CarShop.Application.Models;
using Microsoft.AspNetCore.Identity;
namespace CarShop.Application;

public interface IUserService
{
    Task<IdentityResult> RegisterAsync(RegisterDto dto);
    
    Task<AuthResponseDto> LoginAsync(LoginDto dto);
}
