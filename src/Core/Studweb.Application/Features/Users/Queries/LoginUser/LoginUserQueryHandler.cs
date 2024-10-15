using ErrorOr;
using Studweb.Application.Abstractions.Messaging;
using Studweb.Application.Contracts.Authentication;
using Studweb.Application.Persistance;
using Studweb.Application.Utils;
using Studweb.Domain.Common.Errors;

namespace Studweb.Application.Features.Users.Queries.LoginUser;

public class LoginUserQueryHandler : IQueryHandler<LoginUserQuery, LoginResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;

    public LoginUserQueryHandler(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
    }

    public async Task<ErrorOr<LoginResponse>> Handle(LoginUserQuery request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByEmailAsync(request.Email);

        if (user is null)
        {
            return Errors.User.UserNotFound;
        }

        return new LoginResponse(
            user.Id.Value,
            user.FirstName,
            user.LastName,
            user.Email,
            user.Birthday,
            user.VerificationToken?.ToString());
    }
}