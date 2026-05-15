namespace CarShop.Application.Constants;

public static class CarSort
{
    public const string PriceAsc = "price_asc";
    public const string PriceDesc = "price_desc";
    public const string YearAsc = "year_asc";
    public const string YearDesc = "year_desc";
    public const string MileageAsc = "mileage_asc";
    public const string MileageDesc = "mileage_desc";

    public static string Normalize(string? sort) =>
        sort?.ToLowerInvariant() switch
        {
            PriceDesc => PriceDesc,
            YearAsc => YearAsc,
            YearDesc => YearDesc,
            MileageAsc => MileageAsc,
            MileageDesc => MileageDesc,
            _ => PriceAsc
        };
} 