using FluentValidation.TestHelper;
using Studweb.Application.Features.Notes.Commands.DeleteNote;

namespace Studweb.Application.UnitTests.Features.Notes.Commands.DeleteNote;

public class DeleteNoteCommandValidatorTests
{
    private readonly DeleteNoteCommandValidator _deleteNoteCommandValidator = new();
    
    [Theory]
    [InlineData(-5)]
    [InlineData(0)]
    [InlineData(null)]
    public async Task Id_Should_NotBeEmpty(int id)
    {
        // Arrange

        var deleteNoteCommand = new DeleteNoteCommand(id);
        
        // Act

        var result = await _deleteNoteCommandValidator.TestValidateAsync(deleteNoteCommand);
        
        // Assert

        result.ShouldHaveValidationErrorFor(x => x.Id);
    }
}