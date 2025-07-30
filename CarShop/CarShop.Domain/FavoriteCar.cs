namespace CarShop.Domain;

public class FavoriteCar
{
    public string UserId { get; set; } = string.Empty;
    public int CarId { get; set; }

    public Car Car { get; set; } = null!;
    public User User { get; set; } = null!;
}