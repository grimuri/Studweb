using Studweb.Application.Features.Users.Commands;
using Studweb.Application.Features.Users.Commands.RegisterUser;
using Studweb.IntegrationTests.TestUtils;

namespace Studweb.IntegrationTests.Users.TestUtils;

public class RegisterUserCommandBuilder
{
    private string _firstName = Constants.RegisterUser.FirstName;
    private string _lastName = Constants.RegisterUser.LastName;
    private string _email = Constants.RegisterUser.Email;
    private string _password = Constants.RegisterUser.Password;
    private string _confirmPassword = Constants.RegisterUser.ConfirmPassword;
    private DateTime _birthday = Constants.RegisterUser.Birthday;

    public static RegisterUserCommandBuilder GivenRegisterUserCommand() => 
        new RegisterUserCommandBuilder();
    
    public RegisterUserCommandBuilder WithInvalidFirstName()
    {
        _firstName = "";
        return this;
    }

    public RegisterUserCommandBuilder WithInvalidLastName()
    {
        _lastName = "";
        return this;
    }

    public RegisterUserCommandBuilder WithInvalidEmail()
    {
        _email = "email.com";
        return this;
    }

    public RegisterUserCommandBuilder WithInvalidPassword()
    {
        _password = "";
        return this;
    }

    public RegisterUserCommandBuilder WithInvalidConfirmPassword()
    {
        _confirmPassword = "";
        return this;
    }

    public RegisterUserCommandBuilder WithInvalidBirthday()
    {
        _birthday = DateTime.UtcNow;
        return this;
    }

    public RegisterUserCommand Build()
    {
        var registerUserCommand = new RegisterUserCommand(
            _firstName,
            _lastName,
            _email,
            _password,
            _confirmPassword,
            _birthday
        );

        return registerUserCommand;
    }

}