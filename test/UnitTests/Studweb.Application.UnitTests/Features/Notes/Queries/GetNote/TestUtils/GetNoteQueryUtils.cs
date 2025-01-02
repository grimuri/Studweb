using Studweb.Application.Features.Notes.Queries.GetNote;

namespace Studweb.Application.UnitTests.Features.Notes.Queries.GetNote.TestUtils;

public static class GetNoteQueryUtils
{
    public static GetNoteQuery GetNoteQuery() =>
        new GetNoteQuery(1);
}