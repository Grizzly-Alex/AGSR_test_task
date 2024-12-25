using Hospital_API.ValueTypes;
using MediatR;

namespace Hospital_API.CQRS.Interfaces;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>
{
}
