using CarShop.Application.Queries;

namespace CarShop.API.Endpoints.Requests;

public sealed record GetCarByIdRequest(int Id)
{
    public GetCarByIdQuery ToQuery() => new GetCarByIdQuery(Id);
}