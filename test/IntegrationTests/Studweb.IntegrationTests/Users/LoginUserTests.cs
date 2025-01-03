using Dapper;
using FluentAssertions;
using Studweb.Application.Features.Users.Commands.LoginUser;
using Studweb.IntegrationTests.Abstractions;
using Studweb.IntegrationTests.Users.TestUtils;
using static Studweb.IntegrationTests.Users.TestUtils.LoginUserCommandBuilder;
using static Studweb.IntegrationTests.Users.TestUtils.RegisterUserCommandBuilder;

namespace Studweb.IntegrationTests.Users;

[Collection("IntegrationTests")]
public class LoginUserTests : BaseIntegrationTest
{
    public LoginUserTests(IntegrationTestWebAppFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task Handle_Should_GenerateJwtTokenAndReturnLoginResponse_WhenCredentialsAreValid()
    {
        // Arrange
        
        var loginUserCommand = GivenLoginUserCommand()
            .Build();
        
        // Act

        var result = await Sender.Send(loginUserCommand);
        
        // Assert

        result.IsError.Should().BeFalse();
        result.Value.Token.Should().NotBe(null);
    }

    [Theory]
    [MemberData(nameof(InvalidLoginUserCommandData))]
    public async Task Handle_Should_ReturnValidationError_When_CommandIsInvalid(LoginUserCommand loginUserCommand)
    {
        // Arrange 
        
        // Act

        var result = await Sender.Send(loginUserCommand);
        
        // Assert

        result.IsError.Should().BeTrue();
    }

    public static IEnumerable<object[]> InvalidLoginUserCommandData()
    {
        return new List<object[]>
        {
            new[] { GivenLoginUserCommand().WithInvalidEmail().Build() },
            new[] { GivenLoginUserCommand().WithInvalidPassword().Build() },
        };
    }
}