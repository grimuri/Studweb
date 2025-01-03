using Studweb.Application.Features.Notes.Commands.UpdateNote;
using Studweb.Application.UnitTests.TestUtils.Constants;

namespace Studweb.Application.UnitTests.Features.Notes.Commands.UpdateNote.TestUtils;

public static class UpdateNoteCommandUtils
{
    public static UpdateNoteCommand UpdateNoteCommand() =>
        new UpdateNoteCommand(
            Constants.UpdateNote.Id,
            Constants.UpdateNote.Title,
            Constants.UpdateNote.Content,
            Constants.UpdateNote.Tags);
}