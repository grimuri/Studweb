using ErrorOr;
using MediatR;

namespace Studweb.Application.Common.Messaging;

public interface IQuery<TResponse> : IRequest<ErrorOr<TResponse>>
{
    
}