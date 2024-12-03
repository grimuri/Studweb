using FluentAssertions;
using Studweb.Application.Features.Users.Commands;
using Studweb.Application.Features.Users.Commands.RegisterUser;
using Studweb.IntegrationTests.Abstractions;
using static Studweb.IntegrationTests.Users.TestUtils.RegisterUserCommandBuilder;

namespace Studweb.IntegrationTests.Users;

public class RegisterUserTests : BaseIntegrationTest
{
    public RegisterUserTests(IntegrationTestWebAppFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task Handle_Should_CreateUserAndInsertToDb_WhenCommandIsValid()
    {
        // Arrange

        var registerUserCommand = GivenRegisterUserCommand()
            .Build();

        // Act

        var result = await Sender.Send(registerUserCommand);

        // Assert

        result.IsError.Should().BeFalse();
        result.Value.Id.Should().NotBe(null);
    }

    [Theory]
    [MemberData(nameof(InvalidRegisterUserCommandData))]
    public async Task Handle_Should_ReturnValidationError_WhenCommandIsInvalid(RegisterUserCommand command)
    {
        // Arrange
        
        // Act

        var result = await Sender.Send(command);
        
        // Assert

        result.IsError.Should().BeTrue();
    }

    public static IEnumerable<object[]> InvalidRegisterUserCommandData()
    {
        return new List<object[]>
        {
            new[] { GivenRegisterUserCommand().WithInvalidFirstName().Build() },
            new[] { GivenRegisterUserCommand().WithInvalidLastName().Build() },
            new[] { GivenRegisterUserCommand().WithInvalidEmail().Build() },
            new[] { GivenRegisterUserCommand().WithInvalidPassword().Build() },
            new[] { GivenRegisterUserCommand().WithInvalidConfirmPassword().Build() },
            new[] { GivenRegisterUserCommand().WithInvalidBirthday().Build() },
        };
    }
}