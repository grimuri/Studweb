using MediatR;
using Studweb.Domain.DomainEvents;
using Studweb.Domain.Primitives;

namespace Studweb.Application.Features.Users.Events;

public sealed class UserRegisteredDomainEventHandler : INotificationHandler<UserRegistered>
{
    public Task Handle(UserRegistered notification, CancellationToken cancellationToken)
    {
        Console.WriteLine(notification.User.Email + " has registered");

        return Unit.Task;
    }
}