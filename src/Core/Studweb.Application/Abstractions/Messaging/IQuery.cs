using MediatR;
using Studweb.Application.Common;

namespace Studweb.Application.Abstractions.Messaging;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>
{
    
}