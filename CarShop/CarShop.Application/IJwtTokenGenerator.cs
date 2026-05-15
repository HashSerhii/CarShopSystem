namespace CarShop.Application;
using CarShop.Domain;

public interface IJwtTokenGenerator
{
    Task<string> GenerateTokenAsync(User user, CancellationToken cancellationToken = default);
}