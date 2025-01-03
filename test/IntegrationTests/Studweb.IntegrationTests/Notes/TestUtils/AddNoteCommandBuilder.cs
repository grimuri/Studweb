using Studweb.Application.Features.Notes.Commands.AddNote;
using Studweb.Domain.Aggregates.Notes.ValueObjects;
using Studweb.IntegrationTests.TestUtils.Constants;

namespace Studweb.IntegrationTests.Notes.TestUtils;

public class AddNoteCommandBuilder
{
    private string _title = Constants.AddNote.Title;
    private string _content = Constants.AddNote.Content;
    private List<Tag> _tags = Constants.AddNote.Tags;

    public static AddNoteCommandBuilder GivenAddNoteCommand() =>
        new AddNoteCommandBuilder();

    public AddNoteCommandBuilder WithInvalidTitle()
    {
        _title = "";
        return this;
    }

    public AddNoteCommandBuilder WithInvalidContent()
    {
        _content = "";
        return this;
    }

    public AddNoteCommand Build()
    {
        var addNoteCommand = new AddNoteCommand(
            _title,
            _content,
            _tags);

        return addNoteCommand;
    } 
}