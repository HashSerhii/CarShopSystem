namespace CarShop.Application.Services;

public interface IUserContext
{
    string? GetCurrentUserId();

    bool IsInRole(string role);
}