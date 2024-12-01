using FluentValidation.TestHelper;
using Studweb.Application.Features.Users.Commands.LoginUser;
using Studweb.Application.UnitTests.Features.Users.Commands.LoginUser.TestUtils;

namespace Studweb.Application.UnitTests.Features.Users.Commands.LoginUser;

public class LoginUserCommandValidatorTests
{
    private LoginUserCommandValidator _loginUserCommandValidator = new();

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void Email_Should_NotBeEmpty(string email)
    {
        // Arrange

        var loginUserCommand = LoginUserCommandUtils.LoginUserCommand()
            with
            {
                Email = email
            };
        
        // Act

        var result = _loginUserCommandValidator.TestValidate(loginUserCommand);
        
        // Assert

        result.ShouldHaveValidationErrorFor(x => x.Email);
    }
    
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void Password_Should_NotBeEmpty(string password)
    {
        // Arrange

        var loginUserCommand = LoginUserCommandUtils.LoginUserCommand()
            with
            {
                Password = password
            };
        
        // Act

        var result = _loginUserCommandValidator.TestValidate(loginUserCommand);
        
        // Assert

        result.ShouldHaveValidationErrorFor(x => x.Password);
    }
}