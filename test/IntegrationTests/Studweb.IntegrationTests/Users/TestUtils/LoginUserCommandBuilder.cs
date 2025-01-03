using Studweb.Application.Features.Users.Commands.LoginUser;
using Studweb.IntegrationTests.TestUtils;
using Studweb.IntegrationTests.TestUtils.Constants;

namespace Studweb.IntegrationTests.Users.TestUtils;

public class LoginUserCommandBuilder
{
    private string _email = Constants.LoginUser.Email;
    private string _password = Constants.LoginUser.Password;

    public static LoginUserCommandBuilder GivenLoginUserCommand() => 
        new LoginUserCommandBuilder();

    public LoginUserCommandBuilder WithInvalidEmail()
    {
        _email = "";
        return this;
    }

    public LoginUserCommandBuilder WithInvalidPassword()
    {
        _password = "";
        return this;
    }

    public LoginUserCommand Build()
    {
        var loginUserCommand = new LoginUserCommand(
            _email,
            _password
        );

        return loginUserCommand;
    }
}