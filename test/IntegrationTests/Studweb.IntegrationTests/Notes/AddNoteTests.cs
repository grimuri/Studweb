using FluentAssertions;
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
}