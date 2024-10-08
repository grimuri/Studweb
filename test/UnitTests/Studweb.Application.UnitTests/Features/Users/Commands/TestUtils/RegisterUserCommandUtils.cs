using Studweb.Application.Features.Users.Commands;
using Studweb.Application.UnitTests.TestUtils.Constants;

namespace Studweb.Application.UnitTests.Features.Users.Commands.TestUtils;

public static class RegisterUserCommandUtils
{
    public static RegisterUserCommand RegisterUserCommand() =>
        new RegisterUserCommand(
            Constants.RegisterUser.FirstName,
            Constants.RegisterUser.LastName,
            Constants.RegisterUser.Email,
            Constants.RegisterUser.Password,
            Constants.RegisterUser.ConfirmPassword,
            Constants.RegisterUser.Birthday);
}