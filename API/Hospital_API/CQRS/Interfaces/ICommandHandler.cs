using Hospital_API.ValueTypes;
using MediatR;

namespace Hospital_API.CQRS.Interfaces;


public interface ICommandHandler<TCommand> : IRequestHandler<TCommand, Result>
where TCommand : ICommand
{
}

public interface ICommandHandler<TCommand, TResponse> : IRequestHandler<TCommand, Result<TResponse>>
    where TCommand : ICommand<TResponse>
{
}

