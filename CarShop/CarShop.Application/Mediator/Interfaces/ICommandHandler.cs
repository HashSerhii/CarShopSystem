namespace CarShop.Application.Mediator.Interfaces;

public interface ICommandHandler<T>
{
    Task ExecuteAsync(T command, CancellationToken cancellationToken);
}

public interface ICommandHandler<T, TResult>
{
    Task<TResult> ExecuteAsync(T command, CancellationToken cancellationToken);
}