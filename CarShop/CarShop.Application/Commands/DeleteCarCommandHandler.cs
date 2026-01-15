using CarShop.Application.Mediator.Interfaces;
using CarShop.Application.Repositories;

namespace CarShop.Application.Commands;

public sealed record DeleteCarCommandHandler : ICommandHandler<DeleteCarCommand,bool>
{
    private readonly IGetCarsRepository _repository;

    public DeleteCarCommandHandler(IGetCarsRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> ExecuteAsync(DeleteCarCommand command, CancellationToken cancellationToken)
    {
        var result = await _repository.DeleteCarAsync(command.Id, cancellationToken);
        
        return result;
    }
}