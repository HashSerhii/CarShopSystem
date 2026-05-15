using System.Security.Claims;
using CarShop.Application.Services;
using Microsoft.AspNetCore.Http;

namespace CarShop.Infrastructure.Authentication;

public class UserContext: IUserContext
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserContext(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string? GetCurrentUserId()
    {
        var user = _httpContextAccessor.HttpContext?.User;

        return user?.FindFirstValue(ClaimTypes.NameIdentifier);
    }

    public bool IsInRole(string role)
    {
        var user = _httpContextAccessor.HttpContext?.User;

        return user?.IsInRole(role) ?? false;
    }
}