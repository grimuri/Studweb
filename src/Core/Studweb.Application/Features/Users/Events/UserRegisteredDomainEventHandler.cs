using System.Net;
using System.Net.Mail;
using FluentEmail.Core;
using MediatR;
using Studweb.Application.Persistance;
using Studweb.Domain.DomainEvents;

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

        var user = await _userRepository.GetByEmailAsync(notification.User.Email);
        var userId = user.Id.Value;
        await _userRepository.EditVerificationTokenAsync(userId, verificationTokenId, cancellationToken);

        await _email
            .To(notification.User.Email)
            .Subject("Email verification for Studweb")
            .Body($"To verify your email address pass the code: {verificationToken.Value}")
            .SendAsync();
        
    }
}