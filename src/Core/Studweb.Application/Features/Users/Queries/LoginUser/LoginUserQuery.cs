using Studweb.Application.Abstractions.Messaging;
using Studweb.Application.Contracts.Authentication;

namespace Studweb.Application.Features.Users.Queries.LoginUser;

public record LoginUserQuery(
    string Email,
    string Password) : IQuery<LoginResponse>;