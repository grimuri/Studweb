using Newtonsoft.Json;
using Studweb.Domain.Aggregates.Notes.ValueObjects;
using Studweb.Domain.Aggregates.Users.ValueObjects;
using Studweb.Domain.Common.Primitives;

namespace Studweb.Domain.Aggregates.Notes;

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

    public Note Load(
        NoteId id,
        string title,
        string content,
        DateTime createdOnUtc,
        DateTime lastModifiedOnUtc,
        List<Tag> tags,
        UserId userId
    )
    {
        Id = id;
        Title = title;
        Content = content;
        CreatedOnUtc = createdOnUtc;
        LastModifiedOnUtc = lastModifiedOnUtc;
        Tags = tags;
        UserId = userId;

        return this;
    }

    public static Note Empty() => new Note();

    public Note Update(
        string title,
        string content,
        List<Tag> tags
    )
    {
        Title = title;
        Content = content;
        Tags = tags;
        LastModifiedOnUtc = DateTime.UtcNow;
        
        return this;
    }
}