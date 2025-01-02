using FluentValidation.TestHelper;
using Studweb.Application.Features.Notes.Queries.GetNote;
using Studweb.Application.UnitTests.Features.Notes.Queries.GetNote.TestUtils;

namespace Studweb.Application.UnitTests.Features.Notes.Queries.GetNote;

public class GetNoteQueryValidatorTests
{
    private readonly GetNoteQueryValidator _getNoteQueryValidator = new ();

    [Theory]
    [InlineData(-5)]
    [InlineData(0)]
    [InlineData(null)]
    public async Task Id_Should_NotBeEmpty(int id)
    {
        // Arrange

        var getNoteQuery = GetNoteQueryUtils.GetNoteQuery()
            with
            {
                Id = id
            };
        
        // Act

        var result = await _getNoteQueryValidator.TestValidateAsync(getNoteQuery);
        
        // Assert

        result.ShouldHaveValidationErrorFor(x => x.Id);
    }
}