using ErrorOr;
using MediatR;

namespace Studweb.Application.Abstractions.Messaging;

public interface IQuery<TResponse> : IRequest<ErrorOr<TResponse>>
{
    
}