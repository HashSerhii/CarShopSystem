using CarShop.Domain;

namespace CarShop.Application.Repositories;

public interface IUserRepository
{
    Task<User?> GetByEmailAsync(string email);
    Task AddAsync(User user);
}