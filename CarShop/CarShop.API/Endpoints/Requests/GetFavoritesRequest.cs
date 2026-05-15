using CarShop.Application.Queries;

namespace CarShop.API.Endpoints.Requests;

public sealed record GetFavoritesRequest(int Page = 1, int PageSize = 20)
{
    public GetFavoritesQuery ToQuery() => new(
        Math.Clamp(Page, 1, int.MaxValue),
        Math.Clamp(PageSize, 1, 100));
}
