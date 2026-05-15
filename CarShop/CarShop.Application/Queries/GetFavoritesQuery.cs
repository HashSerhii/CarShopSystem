namespace CarShop.Application.Queries;

public sealed record GetFavoritesQuery(string UserId, int Page = 1, int PageSize = 20); 