using Studweb.Application.Abstractions.Messaging;
using Studweb.Application.Contracts.Authentication;

namespace Studweb.Application.Features.Users.Commands;

public record RegisterUserCommand(
    string Name,
    string Surname,
    string Email,
    string Password,
    string ConfirmPassword,
    DateTime? Birthday) : ICommand<RegisterResponse>;