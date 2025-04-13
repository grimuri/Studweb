using ErrorOr;
using Studweb.Application.Common.Messaging;
using Studweb.Application.Contracts.Authentication;
using Studweb.Application.Persistance;
using Studweb.Application.Utils;
using Studweb.Domain.Common.Errors;

namespace Studweb.Application.Features.Users.Commands.LoginUser;

public class LoginUserCommandHandler : ICommandHandler<LoginUserCommand, LoginResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtProvider _jwtProvider;

    public LoginUserCommandHandler(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        IJwtProvider jwtProvider)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _jwtProvider = jwtProvider;
    }

    public async Task<ErrorOr<LoginResponse>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByEmailAsync(request.Email);

        if (user is null)
        {
            return Errors.User.IncorrectData;
        }

        if (user.VerifiedOnUtc is null)
        {
            return Errors.User.UserNotVerified;
        }

        if (!_passwordHasher.Verify(request.Password, user.Password))
        {
            return Errors.User.IncorrectData;
        }

        var token = _jwtProvider.Generate(user);

        return new LoginResponse(
            user.Id.Value,
            user.FirstName,
            user.LastName,
            user.Email,
            user.Birthday,
            token);
    }
}