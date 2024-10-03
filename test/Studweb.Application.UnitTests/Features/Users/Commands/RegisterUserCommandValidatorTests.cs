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
            with
            {
                Email = email
            };

        // Act

        var result = _registerUserCommandValidator.TestValidate(registerUserCommand);

        // Assert

        result.ShouldHaveValidationErrorFor(x => x.Email);
    }

    [Theory]
    [InlineData("test.com")]
    [InlineData("@gmail.com")]
    [InlineData("test@test@gmail.com")]
    public void Email_ShouldBeValid(string email)
    {
        // Arrange

        var registerUserCommand = RegisterUserCommandUtils.RegisterUserCommand()
            with
            {
                Email = email
            };

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
            with
            {
                FirstName = firstName
            };

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
            with
            {
                Lastname = lastName
            };

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
            with
            {
                Password = password
            };

        // Act

        var result = _registerUserCommandValidator.TestValidate(registerUserCommand);

        // Assert

        result.ShouldHaveValidationErrorFor(x => x.Password);
    }

    [Theory]
    [InlineData("a")]
    [InlineData("aa")]
    [InlineData("aaa")]
    [InlineData("aaaa")]
    [InlineData("aaaaa")]
    [InlineData("aaaaaa")]
    [InlineData("aaaaaaa")]
    public void Password_ShouldNotBeShorterThan8Characters(string password)
    {
        // Arrange

        var registerUserCommand = RegisterUserCommandUtils.RegisterUserCommand()
            with
            {
                Password = password
            };

        // Act

        var result = _registerUserCommandValidator.TestValidate(registerUserCommand);

        // Assert

        result.ShouldHaveValidationErrorFor(x => x.Password);
    }

    [Theory]
    [InlineData("qwerty123", "qwerty12")]
    [InlineData("qwerty1234", "qwerty1233")]
    [InlineData("qwerty123", "qwerty1234")]
    public void ConfirmPassword_ShouldBeTheSameAsPassword(string confirmPassword, string password)
    {
        // Arrange

        var registerUserCommand = RegisterUserCommandUtils.RegisterUserCommand()
            with
            {
                Password = password,
                ConfirmPassword = confirmPassword
            };

        // Act

        var result = _registerUserCommandValidator.TestValidate(registerUserCommand);

        // Assert

        result.ShouldHaveValidationErrorFor(x => x.ConfirmPassword);
    }

    [Theory]
    [InlineData(null)]
    public void Birthday_ShouldNotBeEmpty(DateTime birthday)
    {
        // Arrange

        var registerUserCommand = RegisterUserCommandUtils.RegisterUserCommand()
            with
            {
                Birthday = birthday
            };

        // Act

        var result = _registerUserCommandValidator.TestValidate(registerUserCommand);

        // Assert

        result.ShouldHaveValidationErrorFor(x => x.Birthday);
    }

    [Theory]
    [MemberData(nameof(Birthdays))]
    public void Birthday_ShouldIndicateUserIsAtLeast13YearsOld(DateTime birthday)
    {
        // Arrange

        var registerUserCommand = RegisterUserCommandUtils.RegisterUserCommand()
            with
            {
                Birthday = birthday
            };

        // Act

        var result = _registerUserCommandValidator.TestValidate(registerUserCommand);

        // Assert

        result.ShouldHaveValidationErrorFor(x => x.Birthday);
    }

    public static IEnumerable<object[]> Birthdays => new List<object[]>
    {
        new object[] { DateTime.UtcNow },
        new object[] { DateTime.UtcNow.AddYears(-12) },
        new object[] { DateTime.UtcNow.AddYears(-5) }
    };
}