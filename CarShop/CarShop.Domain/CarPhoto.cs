namespace CarShop.Domain;

public class CarPhoto
{
    public int Id { get; set; }
    public int CarId { get; set; }
    public string Url { get; set; } = string.Empty;

    public Car Car { get; set; } = null!;
}