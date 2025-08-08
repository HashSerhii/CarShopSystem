namespace CarShop.Application;
using CarShop.Domain;

public interface IJwtTokenGenerator
{
    string GenerateToken(User user);
}