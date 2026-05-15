namespace CarShop.Application.Queries;

public sealed record GetFavoritesQuery(int Page = 1, int PageSize = 20);
