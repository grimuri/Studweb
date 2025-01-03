using FluentValidation.TestHelper;
using Studweb.Application.Features.Notes.Commands.UpdateNote;
using Studweb.Application.UnitTests.Features.Notes.Commands.UpdateNote.TestUtils;

namespace Studweb.Application.UnitTests.Features.Notes.Commands.UpdateNote;

public class UpdateNoteCommandValidatorTests
{
    private readonly UpdateNoteCommandValidator _updateNoteCommandValidator = new();

    [Theory]
    [InlineData(-5)]
    [InlineData(0)]
    [InlineData(null)]
    public async Task Id_Should_NotBeEmpty(int id)
    {
        // Arrange

        var getNoteQuery = UpdateNoteCommandUtils.UpdateNoteCommand()
            with
            {
                Id = id
            };
        
        // Act

        var result = await _updateNoteCommandValidator.TestValidateAsync(getNoteQuery);
        
        // Assert

        result.ShouldHaveValidationErrorFor(x => x.Id);
    }
    
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public async Task Title_Should_NotBeEmpty(string title)
    {
        // Arrange

        var updateNoteCommand = UpdateNoteCommandUtils.UpdateNoteCommand()
            with
            {
                Title = title
            };
        
        // Act

        var result = await _updateNoteCommandValidator
            .TestValidateAsync(updateNoteCommand);
        
        // Assert

        result.ShouldHaveValidationErrorFor(x => x.Title);
    }
    
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public async Task Content_Should_NotBeEmpty(string content)
    {
        // Arrange

        var updateNoteCommand = UpdateNoteCommandUtils.UpdateNoteCommand()
            with
            {
                Content = content
            };
        
        // Act

        var result = await _updateNoteCommandValidator
            .TestValidateAsync(updateNoteCommand);
        
        // Assert

        result.ShouldHaveValidationErrorFor(x => x.Content);
    }
}