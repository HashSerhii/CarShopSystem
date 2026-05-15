namespace CarShop.Domain;

public class Car
{
    public int Id { get; set; }
    public int BrandId { get; set; }
    public string Model { get; set; } = string.Empty;
    public int Year { get; set; }
    public decimal Price { get; set; }
    public string Description { get; set; } = string.Empty;
    public string OwnerId { get; set; } = string.Empty;

    
    public Brand Brand { get; set; } = null!;
    public User Owner { get; set; } = null!;
    public ICollection<CarPhoto> Photos { get; set; } = new List<CarPhoto>();
    public ICollection<FavoriteCar> FavoriteByUsers { get; set; } = [];
}