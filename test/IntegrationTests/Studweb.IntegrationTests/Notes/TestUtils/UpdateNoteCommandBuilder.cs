using Studweb.Application.Features.Notes.Commands.UpdateNote;
using Studweb.Domain.Aggregates.Notes.ValueObjects;
using Studweb.IntegrationTests.TestUtils.Constants;

namespace Studweb.IntegrationTests.Notes.TestUtils;

public class UpdateNoteCommandBuilder
{
    private int _id = Constants.UpdateNote.Id;
    private string _title = Constants.UpdateNote.Title;
    private string _content = Constants.UpdateNote.Content;
    private List<Tag> _tags = Constants.UpdateNote.Tags;

    public static UpdateNoteCommandBuilder GivenUpdateNoteCommand() =>
        new UpdateNoteCommandBuilder();

    public UpdateNoteCommandBuilder WithSpecificId(int id)
    {
        _id = id;
        return this;
    }
    
    public UpdateNoteCommandBuilder WithInvalidId()
    {
        _id = -5;
        return this;
    }
    
    public UpdateNoteCommandBuilder WithInvalidTitle()
    {
        _title = "";
        return this;
    }

    public UpdateNoteCommandBuilder WithInvalidContent()
    {
        _content = "";
        return this;
    }

    public UpdateNoteCommand Build()
    {
        var updateNoteCommand = new UpdateNoteCommand(
            _id,
            _title,
            _content,
            _tags);
        return updateNoteCommand;
    }
}