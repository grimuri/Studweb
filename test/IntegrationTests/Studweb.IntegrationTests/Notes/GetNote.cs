using FluentAssertions;
using Studweb.Application.Features.Notes.Queries.GetNote;
using Studweb.Domain.Common.Errors;
using Studweb.Infrastructure.Utils.Extensions;
using Studweb.IntegrationTests.Abstractions;

namespace Studweb.IntegrationTests.Notes;

[Collection("IntegrationTests")]
public class GetNote : BaseIntegrationTest
{
    public GetNote(IntegrationTestWebAppFactory factory) : base(factory)
    {
    }
    
    [Fact]
    public async Task Handle_Should_ReturnErrorUserNotAuthenticated_WhenUserIsNotAuthenticated()
    {
        // Arrange
        
        var getNoteQuery = new GetNoteQuery(1);
        HttpContextAccessor.HttpContext = null;
        
        // Act

        var result = await Sender.Send(getNoteQuery);
        
        // Assert
        
        result.IsError.Should().BeTrue();
        result.Errors.Should().Contain(Errors.User.UserNotAuthenticated);
    }

    [Fact]
    public async Task Handle_Should_ReturnErrorNotFound_WhenNoteWithThatIdDoesNotExist()
    {
        // Arrange
        
        var getNoteQuery = new GetNoteQuery(999);
        
        // Act

        var result = await Sender.Send(getNoteQuery);
        
        // Assert
        
        result.IsError.Should().BeTrue();
        result.Errors.Should().Contain(Errors.Note.NotFound);
    }

    [Fact]
    public async Task Handle_Should_ReturnErrorAccessDenied_WhenUserDoesNotHaveAccessToTheNote()
    {
        // Arrange
        
        var getNoteQuery = new GetNoteQuery(1);
        HttpContextAccessor.HttpContext = SetAuthenticatedUser(5);
        
        // Act

        var result = await Sender.Send(getNoteQuery);
        
        // Assert

        result.IsError.Should().BeTrue();
        result.Errors.Should().Contain(Errors.Note.AccessDenied);
    }

    [Fact]
    public async Task Handle_Should_ReturnTheNote_WhenTheDataIsCorrect()
    {
        // Arrange
        
        var getNoteQuery = new GetNoteQuery(1);
        
        // Act

        var result = await Sender.Send(getNoteQuery);
        
        // Assert

        result.IsError.Should().BeFalse();
    }
    
    
}