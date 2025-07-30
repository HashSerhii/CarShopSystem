using Microsoft.AspNetCore.Identity;

namespace CarShop.Domain;


public class User : IdentityUser
{
    public ICollection<Car> Cars { get; set; } = new List<Car>();
    public ICollection<FavoriteCar> FavoriteCars { get; set; } = new List<FavoriteCar>();
}