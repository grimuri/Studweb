using ErrorOr;
using MediatR;

namespace Studweb.Application.Abstractions.Messaging;

public interface IQueryHandler<TQuery, TResponse> : IRequestHandler<TQuery, ErrorOr<TResponse>>
    where TQuery : IQuery<TResponse>
{
    //Task<ErrorOr<TResponse>> Handle(TQuery query, CancellationToken cancellationToken);
}