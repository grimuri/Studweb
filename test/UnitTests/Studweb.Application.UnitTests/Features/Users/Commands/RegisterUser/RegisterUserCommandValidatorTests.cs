using FluentValidation.TestHelper;
using Studweb.Application.Features.Users.Commands.RegisterUser;
using Studweb.Application.UnitTests.Features.Users.Commands.RegisterUser.TestUtils;

namespace Studweb.Application.UnitTests.Features.Users.Commands.RegisterUser;

public class RegisterUserCommandValidatorTests
{
    private RegisterUserCommandValidator _registerUserCommandValidator = new();

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void Email_Should_NotBeEmpty(string email)
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
    public void Email_Should_BeValid(string email)
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
    public void FirstName_Should_NotBeEmpty(string firstName)
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
    public void LastName_Should_NotBeEmpty(string lastName)
    {
        // Arrange

        var registerUserCommand = RegisterUserCommandUtils.RegisterUserCommand()
            with
            {
                LastName = lastName
            };

        // Act

        var result = _registerUserCommandValidator.TestValidate(registerUserCommand);

        // Assert

        result.ShouldHaveValidationErrorFor(x => x.LastName);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void Password_Should_NotBeEmpty(string password)
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
    public void Password_Should_NotBeShorterThan8Characters(string password)
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
    public void ConfirmPassword_Should_BeTheSameAsPassword(string confirmPassword, string password)
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
    public void Birthday_Should_NotBeEmpty(DateTime birthday)
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
    public void Birthday_Should_IndicateUserIsAtLeast13YearsOld(DateTime birthday)
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