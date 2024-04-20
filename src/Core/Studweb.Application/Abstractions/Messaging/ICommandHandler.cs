using ErrorOr;
using MediatR;

namespace Studweb.Application.Abstractions.Messaging;

public interface ICommandHandler<TCommand> : IRequestHandler<TCommand, ErrorOr<Unit>>
    where TCommand : ICommand
{
    //Task<ErrorOr<Unit>> Handle(TCommand command, CancellationToken cancellationToken);
}

public interface ICommandHandler<TCommand, TResponse> : IRequestHandler<TCommand, ErrorOr<TResponse>>
    where TCommand : ICommand<TResponse>
{
    //Task<ErrorOr<TResponse>> Handle(TCommand command, CancellationToken cancellationToken);
}