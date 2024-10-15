using Studweb.Application.Abstractions.Messaging;
using Studweb.Application.Contracts.Authentication;

namespace Studweb.Application.Features.Users.Commands.RegisterUser;

public record RegisterUserCommand(
    string FirstName,
    string LastName,
    string Email,
    string Password,
    string ConfirmPassword,
    DateTime Birthday) : ICommand<RegisterResponse>;