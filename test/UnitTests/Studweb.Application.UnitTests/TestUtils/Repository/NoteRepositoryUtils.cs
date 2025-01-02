using System.Collections;
using Studweb.Domain.Aggregates.Notes;
using Studweb.Domain.Aggregates.Notes.ValueObjects;
using Studweb.Domain.Aggregates.Users.ValueObjects;

namespace Studweb.Application.UnitTests.TestUtils.Repository;

public static class NoteRepositoryUtils
{
    private const int NumberOfNotes = 3;
    
    public static IEnumerable<Note> GetExampleNotes()
    {
        var notes = new List<Note>();

        for (int i = 1; i <= NumberOfNotes; i++)
        {
            notes.Add(
                Note.Create(
                    NoteId.Create(i),
                    $"Title{i}",
                    $"Content{i}",
                    new()
                    {
                        Tag.Create($"Tag{i+1}"),
                        Tag.Create($"Tag{i+2}")
                    },
                    UserId.Create(1)
                    )
                );
        }

        return notes;
    }

    public static Note GetExampleNote() =>
        Note.Create(
            NoteId.Create(1),
            "Title",
            "Content",
            new()
            {
                Tag.Create("Tag1"),
                Tag.Create("Tag2")
            },
            UserId.Create(1)
        );
}