using FluentAssertions;
using Studweb.Application.Features.Notes.Commands.AddNote;
using Studweb.Domain.Common.Errors;
using Studweb.IntegrationTests.Abstractions;

using static Studweb.IntegrationTests.Notes.TestUtils.AddNoteCommandBuilder;

namespace Studweb.IntegrationTests.Notes;

[Collection("IntegrationTests")]
public class AddNoteTests : BaseIntegrationTest
{
    public AddNoteTests(IntegrationTestWebAppFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task Handle_Should_CreateNoteAndReturnId_WhenCommandIsValid()
    {
        // Arrange

        var addNoteCommand = GivenAddNoteCommand()
            .Build();
        
        // Act
        
        var result = await Sender.Send(addNoteCommand);
        
        // Assert

        result.IsError.Should().BeFalse();
        result.Value.Id.Should().NotBe(null);
    }

    [Fact]
    public async Task Handle_Should_ReturnErrorUserNotAuthenticated_WhenUserIsNotAuthenticated()
    {
        // Arrange

        var addNoteCommand = GivenAddNoteCommand()
            .Build();
        HttpContextAccessor.HttpContext = null;
        
        // Act
        
        var result = await Sender.Send(addNoteCommand);
        
        // Assert

        result.IsError.Should().BeTrue();
        result.Errors.Should().Contain(Errors.User.UserNotAuthenticated);
    }

    [Theory]
    [MemberData(nameof(InvalidAddNoteCommandData))]
    public async Task Handle_Should_ReturnValidationError_WhenCommandIsInvalid(AddNoteCommand command)
    {
        // Arrange
        
        // Act

        var result = await Sender.Send(command);
        
        // Assert

        result.IsError.Should().BeTrue();
    }

    public static IEnumerable<object[]> InvalidAddNoteCommandData()
    {
        return new List<object[]>
        {
            new[] { GivenAddNoteCommand().WithInvalidTitle().Build() },
            new[] { GivenAddNoteCommand().WithInvalidContent().Build() }
        };
    }
}