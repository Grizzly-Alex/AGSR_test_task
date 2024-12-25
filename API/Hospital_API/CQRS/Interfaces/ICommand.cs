using Hospital_API.ValueTypes;
using MediatR;

namespace Hospital_API.CQRS.Interfaces;

public interface ICommand : IRequest<Result>
{
}

public interface ICommand<TResponse> : IRequest<Result<TResponse>>
{
}
