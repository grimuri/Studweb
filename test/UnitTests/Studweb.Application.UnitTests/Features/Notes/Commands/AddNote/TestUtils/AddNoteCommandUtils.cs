using Studweb.Application.Features.Notes.Commands.AddNote;
using Studweb.Application.UnitTests.TestUtils.Constants;

namespace Studweb.Application.UnitTests.Features.Notes.Commands.AddNote.TestUtils;

public static class AddNoteCommandUtils
{
    public static AddNoteCommand AddNoteCommand() => 
        new AddNoteCommand(
            Constants.AddNote.Title,
            Constants.AddNote.Content,
            Constants.AddNote.Tags
        );
    
}