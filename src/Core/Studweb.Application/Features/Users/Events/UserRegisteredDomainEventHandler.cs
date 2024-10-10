using System.Net;
using System.Net.Mail;
using FluentEmail.Core;
using MediatR;
using Studweb.Application.Persistance;
using Studweb.Domain.DomainEvents;
using Studweb.Domain.Entities;
using Studweb.Domain.Primitives;

namespace Studweb.Application.Features.Users.Events;

public sealed class UserRegisteredDomainEventHandler : INotificationHandler<UserRegistered>
{
    private readonly IFluentEmail _email;
    private ITokenRepository _tokenRepository;
    private IUserRepository _userRepository;

    public UserRegisteredDomainEventHandler(IFluentEmail email, ITokenRepository tokenRepository, IUserRepository userRepository)
    {
        _email = email;
        _tokenRepository = tokenRepository;
        _userRepository = userRepository;
    }

    public async Task Handle(UserRegistered notification, CancellationToken cancellationToken)
    {
        Console.WriteLine(notification.User.Email + " has registered");
        
        var verificationToken = notification.User.VerificationToken;
        var verificationTokenId = await _tokenRepository.AddTokenAsync(verificationToken, cancellationToken);

        var userId = await _userRepository.GetByEmailAsync(notification.User.Email);
        await _userRepository.EditVerificationTokenAsync(userId, verificationTokenId, cancellationToken);

        await _email
            .To(notification.User.Email)
            .Subject("Email verification for Studweb")
            .Body($"To verify your email address pass the code: {verificationToken.Value}")
            .SendAsync();
        
        // var client = new SmtpClient("smtp.mailtrap.io", 25)
        // {
        //     Credentials = new NetworkCredential("ssdsasf23d7c3d", "6898c9erf238"),
        //     EnableSsl = true
        // };
        // client.Send("from@example.com", $@"{notification.User.Email}", "Hello world", "testbody");
    }
}