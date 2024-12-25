using Hospital_API.ValueTypes;
using MediatR;

namespace Hospital_API.CQRS.Interfaces;

public interface IQueryHandler<TQuery, TResponse> : IRequestHandler<TQuery, Result<TResponse>>
    where TQuery : IQuery<TResponse>
{
}
