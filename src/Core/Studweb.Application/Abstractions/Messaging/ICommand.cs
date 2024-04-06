using MediatR;
using Studweb.Application.Common;

namespace Studweb.Application.Abstractions.Messaging;

public interface ICommand : IRequest<Result>
{
    
}

public interface ICommand<TResponse> : IRequest<Result<TResponse>>
{
    
}
