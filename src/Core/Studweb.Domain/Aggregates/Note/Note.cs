using Newtonsoft.Json;
using Studweb.Domain.Aggregates.Note.ValueObjects;
using Studweb.Domain.Aggregates.User.ValueObjects;
using Studweb.Domain.Primitives;

namespace Studweb.Domain.Aggregates.Note;

public sealed class Note : AggregateRoot<NoteId>
{
    public string Title { get; set; }
    public string Content { get; set; }
    public DateTime CreatedOnUtc { get; private set; }
    public DateTime LastModifiedOnUtc { get; private set; }
    public List<Tag> Tags { get; set; }
    public UserId UserId { get; private set; }
    
    private Note(NoteId id = default) : base(id)
    {
    }

    [JsonConstructor]
    private Note(
        NoteId noteId,
        string title,
        string content,
        List<Tag> tags,
        UserId userId
    ) : base(noteId)
    {
        Title = title;
        Content = content;
        CreatedOnUtc = DateTime.UtcNow;
        LastModifiedOnUtc = DateTime.UtcNow;
        Tags = tags;
        UserId = userId;
    }

    public static Note Create(
        NoteId? noteId,
        string title,
        string content,
        List<Tag> tags,
        UserId userId
    )
    {
        if (noteId is null)
        {
            noteId = NoteId.Create();
        }

        var note = new Note(
            noteId,
            title,
            content,
            tags,
            userId);

        return note;
    }
}