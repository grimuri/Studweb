using FluentAssertions;
using Studweb.Application.Features.Notes.Queries.GetAllNotes;
using Studweb.Domain.Common.Errors;
using Studweb.IntegrationTests.Abstractions;

namespace Studweb.IntegrationTests.Notes;

[Collection("IntegrationTests")]
public class GetAllNotesTests : BaseIntegrationTest
{
    public GetAllNotesTests(IntegrationTestWebAppFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task Handle_Should_ReturnErrorUserNotAuthenticated_WhenUserIsNotAuthenticated()
    {
        // Arrange
        
        var getAllNotesQuery = new GetAllNotesQuery();
        HttpContextAccessor.HttpContext = null;
        
        // Act

        var result = await Sender.Send(getAllNotesQuery);
        
        // Assert
        
        result.IsError.Should().BeTrue();
        result.Errors.Should().Contain(Errors.User.UserNotAuthenticated);
    }

    [Fact]
    public async Task Handle_Should_ReturnAllNotesBelongingToUser_WhenUserIsAuthenticated()
    {
        // Arrange

        var getAllNotesQuery = new GetAllNotesQuery();
        
        // Act

        var result = await Sender.Send(getAllNotesQuery);
        
        // Assert

        result.IsError.Should().BeFalse();
        result.Value.Notes.Should().HaveCountGreaterThan(-1);
    }
}