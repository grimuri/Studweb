using FluentAssertions;
using Studweb.Domain.Common.Errors;
using Studweb.IntegrationTests.Abstractions;

using static Studweb.IntegrationTests.Notes.TestUtils.UpdateNoteCommandBuilder;

namespace Studweb.IntegrationTests.Notes;

[Collection("IntegrationTests")]
public sealed class UpdateNoteTests : BaseIntegrationTest
{
    public UpdateNoteTests(IntegrationTestWebAppFactory factory) : base(factory)
    {
    }
    
    [Fact]
    public async Task Handle_Should_ReturnErrorUserNotAuthenticated_WhenUserIsNotAuthenticated()
    {
        // Arrange

        var updateNoteCommand = GivenUpdateNoteCommand()
            .Build();
        HttpContextAccessor.HttpContext = null;
        
        // Act

        var result = await Sender.Send(updateNoteCommand);
        
        // Assert
        
        result.IsError.Should().BeTrue();
        result.Errors.Should().Contain(Errors.User.UserNotAuthenticated);
    }
    
    [Fact]
    public async Task Handle_Should_ReturnErrorNotFound_WhenNoteWithThatIdDoesNotExist()
    {
        // Arrange

        var updateNoteCommand = GivenUpdateNoteCommand()
            .WithSpecificId(3)
            .Build();
        
        // Act

        var result = await Sender.Send(updateNoteCommand);
        
        // Assert
        
        result.IsError.Should().BeTrue();
        result.Errors.Should().Contain(Errors.Note.NotFound);
    }
    
    [Fact]
    public async Task Handle_Should_ReturnErrorAccessDenied_WhenUserDoesNotHaveAccessToTheNote()
    {
        // Arrange

        var updateNoteCommand = GivenUpdateNoteCommand()
            .Build();
        HttpContextAccessor.HttpContext = SetAuthenticatedUser(5);
        
        // Act

        var result = await Sender.Send(updateNoteCommand);
        
        // Assert
        
        result.IsError.Should().BeTrue();
        result.Errors.Should().Contain(Errors.Note.AccessDenied);
    }

    [Fact]
    public async Task Handle_Should_UpdateAndReturnTheNote_WhenTheNoteIsValid()
    {
        // Arrange

        var updateNoteCommand = GivenUpdateNoteCommand()
            .Build();
        
        // Act
        
        var result = await Sender.Send(updateNoteCommand);
        
        // Assert
        
        result.IsError.Should().BeFalse();
        result.Value.Id.Should().NotBe(null);
    }
    
    public static IEnumerable<object[]> InvalidUpdateNoteCommandData()
    {
        return new List<object[]>
        {
            new []{ GivenUpdateNoteCommand().WithInvalidId().Build() },
            new[] { GivenUpdateNoteCommand().WithInvalidTitle().Build() },
            new[] { GivenUpdateNoteCommand().WithInvalidContent().Build() }
        };
    }
}