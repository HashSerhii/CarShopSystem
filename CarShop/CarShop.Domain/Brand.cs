namespace CarShop.Domain;

public class Brand
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    public ICollection<Car> Cars { get; set; } = new List<Car>();
}