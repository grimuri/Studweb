using Studweb.Application.Features.Users.Commands.LoginUser;
using Studweb.Application.UnitTests.TestUtils.Constants;

namespace Studweb.Application.UnitTests.Features.Users.Commands.LoginUser.TestUtils;

public static class LoginUserCommandUtils
{
    public static LoginUserCommand LoginUserCommand() =>
        new LoginUserCommand(
            Constants.LoginUser.Email,
            Constants.LoginUser.Password);
}