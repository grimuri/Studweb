using FluentValidation;
using FluentValidation.TestHelper;
using Studweb.Application.Features.Users.Commands;
using Studweb.Application.UnitTests.Features.Users.Commands.TestUtils;
using Studweb.Application.UnitTests.TestUtils;

namespace Studweb.Application.UnitTests.Features.Users.Commands;

public class RegisterUserCommandValidatorTests
{
    private RegisterUserCommandValidator _registerUserCommandValidator = new();

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void Email_ShouldNotBeEmpty(string email)
    {
        // Arrange

        var registerUserCommand = RegisterUserCommandUtils.RegisterUserCommand() 
            with { Email = email };
        
        // Act

        var result = _registerUserCommandValidator.TestValidate(registerUserCommand);
        
        // Assert

        result.ShouldHaveValidationErrorFor(x => x.Email);
    }
    
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void FirstName_ShouldNotBeEmpty(string firstName)
    {
        // Arrange

        var registerUserCommand = RegisterUserCommandUtils.RegisterUserCommand() 
            with { FirstName = firstName};
        
        // Act

        var result = _registerUserCommandValidator.TestValidate(registerUserCommand);
        
        // Assert

        result.ShouldHaveValidationErrorFor(x => x.FirstName);
    }
    
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void LastName_ShouldNotBeEmpty(string lastName)
    {
        // Arrange

        var registerUserCommand = RegisterUserCommandUtils.RegisterUserCommand() 
            with { Lastname = lastName};
        
        // Act

        var result = _registerUserCommandValidator.TestValidate(registerUserCommand);
        
        // Assert

        result.ShouldHaveValidationErrorFor(x => x.Lastname);
    }
    
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void Password_ShouldNotBeEmpty(string password)
    {
        // Arrange

        var registerUserCommand = RegisterUserCommandUtils.RegisterUserCommand() 
            with { Password = password};
        
        // Act

        var result = _registerUserCommandValidator.TestValidate(registerUserCommand);
        
        // Assert

        result.ShouldHaveValidationErrorFor(x => x.Password);
    }
    
    [Theory]
    [InlineData(null)]
    public void Birthday_ShouldNotBeEmpty(DateTime birthday)
    {
        // Arrange

        var registerUserCommand = RegisterUserCommandUtils.RegisterUserCommand() 
            with { Birthday = birthday};
        
        // Act

        var result = _registerUserCommandValidator.TestValidate(registerUserCommand);
        
        // Assert

        result.ShouldHaveValidationErrorFor(x => x.Birthday);
    }
    
    
}