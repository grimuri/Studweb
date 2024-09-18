using System.Net;
using System.Net.Mail;
using FluentEmail.Core;
using MediatR;
using Studweb.Domain.DomainEvents;
using Studweb.Domain.Primitives;

namespace Studweb.Application.Features.Users.Events;

public sealed class UserRegisteredDomainEventHandler : INotificationHandler<UserRegistered>
{
    private readonly IFluentEmail _email;

    public UserRegisteredDomainEventHandler(IFluentEmail email)
    {
        _email = email;
    }

    public async Task Handle(UserRegistered notification, CancellationToken cancellationToken)
    {
        Console.WriteLine(notification.User.Email + " has registered");

        await _email
            .To(notification.User.Email)
            .Subject("Email verification for Studweb")
            .Body($"To verify your email address pass the code: {notification.User.VerificationToken}")
            .SendAsync();
        
        // var client = new SmtpClient("smtp.mailtrap.io", 25)
        // {
        //     Credentials = new NetworkCredential("ssdsasf23d7c3d", "6898c9erf238"),
        //     EnableSsl = true
        // };
        // client.Send("from@example.com", $@"{notification.User.Email}", "Hello world", "testbody");
    }
}