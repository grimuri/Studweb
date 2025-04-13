using Studweb.Application.Common.Messaging;
using Studweb.Application.Contracts.Authentication;

namespace Studweb.Application.Features.Users.Commands.LoginUser;

public record LoginUserCommand(
    string Email,
    string Password) : ICommand<LoginResponse>;