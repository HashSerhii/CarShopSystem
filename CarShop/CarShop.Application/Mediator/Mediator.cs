using CarShop.Application.Mediator.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace CarShop.Application.Mediator;

public class Mediator(
    IServiceScopeFactory scopeFactory) : IMediator
{
    public async Task ExecuteCommand<T>(T command, CancellationToken cancellationToken)
    {
        await using var scope = scopeFactory.CreateAsyncScope();
        var commandHandler = scope.ServiceProvider.GetRequiredService<ICommandHandler<T>>();

        await commandHandler.ExecuteAsync(command, cancellationToken);
    }

    public async Task ExecuteQuery<T>(T query, CancellationToken cancellationToken)
    {
        await using var scope = scopeFactory.CreateAsyncScope();
        var queryHandler = scope.ServiceProvider.GetRequiredService<IQueryHandler<T>>();

        await queryHandler.ExecuteAsync(query, cancellationToken);
    }

    public async Task<TResult> ExecuteCommand<T, TResult>(T command, CancellationToken cancellationToken)
    {
        await using var scope = scopeFactory.CreateAsyncScope();
        var commandHandler = scope.ServiceProvider.GetRequiredService<ICommandHandler<T,TResult>>();

        return await commandHandler.ExecuteAsync(command, cancellationToken);
    }

    public async Task<TResult> ExecuteQuery<T, TResult>(T query, CancellationToken cancellationToken)
    {
        await using var scope = scopeFactory.CreateAsyncScope();
        var queryHandler = scope.ServiceProvider.GetRequiredService<IQueryHandler<T, TResult>>();

        return await queryHandler.ExecuteAsync(query, cancellationToken);
    }
}