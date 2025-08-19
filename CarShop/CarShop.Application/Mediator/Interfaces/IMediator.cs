namespace CarShop.Application.Mediator.Interfaces;

public interface IMediator
{
    Task ExecuteCommand<T>(T command,
        CancellationToken cancellationToken);

    Task ExecuteQuery<T>(T query,
        CancellationToken cancellationToken);

    Task<TResult> ExecuteCommand<T, TResult>(T command,
        CancellationToken cancellationToken);

    Task<TResult> ExecuteQuery<T, TResult>(T query,
        CancellationToken cancellationToken);
}