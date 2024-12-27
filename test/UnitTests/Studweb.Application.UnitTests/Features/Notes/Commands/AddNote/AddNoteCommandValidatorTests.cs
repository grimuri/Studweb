using FluentValidation.TestHelper;
using Studweb.Application.Features.Notes.Commands.AddNote;
using Studweb.Application.UnitTests.Features.Notes.Commands.AddNote.TestUtils;

namespace Studweb.Application.UnitTests.Features.Notes.Commands.AddNote;

public class AddNoteCommandValidatorTests
{
    private AddNoteCommandValidator _addNoteCommandValidator = new();

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void Title_Should_NotBeEmpty(string title)
    {
        // Arrange

        var addNoteCommand = AddNoteCommandUtils.AddNoteCommand()
            with
            {
                Title = title
            };
        
        // Act

        var result = _addNoteCommandValidator.TestValidate(addNoteCommand);
        
        // Assert

        result.ShouldHaveValidationErrorFor(x => x.Title);
    }
    
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void Content_Should_NotBeEmpty(string content)
    {
        // Arrange

        var addNoteCommand = AddNoteCommandUtils.AddNoteCommand()
            with
            {
                Content = content
            };
        
        // Act

        var result = _addNoteCommandValidator.TestValidate(addNoteCommand);
        
        // Assert

        result.ShouldHaveValidationErrorFor(x => x.Content);
    }
}