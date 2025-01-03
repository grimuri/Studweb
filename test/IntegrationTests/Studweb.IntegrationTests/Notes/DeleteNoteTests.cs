using FluentAssertions;
using Studweb.Application.Features.Notes.Commands.DeleteNote;
using Studweb.Domain.Common.Errors;
using Studweb.IntegrationTests.Abstractions;

namespace Studweb.IntegrationTests.Notes;

[Collection("IntegrationTests")]
public sealed class DeleteNoteTests : BaseIntegrationTest
{
    public DeleteNoteTests(IntegrationTestWebAppFactory factory) : base(factory)
    {
    }
    
    [Fact]
    public async Task Handle_Should_ReturnErrorUserNotAuthenticated_WhenUserIsNotAuthenticated()
    {
        // Arrange

        var deleteNoteCommand = new DeleteNoteCommand(2);
        HttpContextAccessor.HttpContext = null;
        
        // Act

        var result = await Sender.Send(deleteNoteCommand);
        
        // Assert
        
        result.IsError.Should().BeTrue();
        result.Errors.Should().Contain(Errors.User.UserNotAuthenticated);
    }
    
    [Fact]
    public async Task Handle_Should_ReturnErrorNotFound_WhenNoteWithThatIdDoesNotExist()
    {
        // Arrange

        var deleteNoteCommand = new DeleteNoteCommand(999);
        
        // Act

        var result = await Sender.Send(deleteNoteCommand);
        
        // Assert
        
        result.IsError.Should().BeTrue();
        result.Errors.Should().Contain(Errors.Note.NotFound);
    }
    
    [Fact]
    public async Task Handle_Should_ReturnErrorAccessDenied_WhenUserDoesNotHaveAccessToTheNote()
    {
        // Arrange

        var deleteNoteCommand = new DeleteNoteCommand(2);
        HttpContextAccessor.HttpContext = SetAuthenticatedUser(5);
        
        // Act

        var result = await Sender.Send(deleteNoteCommand);
        
        // Assert
        
        result.IsError.Should().BeTrue();
        result.Errors.Should().Contain(Errors.Note.AccessDenied);
    }
    
    [Fact]
    public async Task Handle_Should_DeleteNote_WhenTheNoteIsValid()
    {
        // Arrange

        var deleteNoteCommand = new DeleteNoteCommand(3);
        
        // Act
        
        var result = await Sender.Send(deleteNoteCommand);
        
        // Assert
        
        result.IsError.Should().BeFalse();
        result.Value.Id.Should().NotBe(null);
    }
}