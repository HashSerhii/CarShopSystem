namespace CarShop.Application.Commands;

public sealed record RemoveFavoriteCommand(string UserId, int CarId);
