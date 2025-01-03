using Studweb.Domain.Aggregates.Notes;
using Studweb.Domain.Aggregates.Notes.ValueObjects;
using Studweb.Domain.Aggregates.Users.ValueObjects;

namespace Studweb.Infrastructure.Utils.TempClasses;

public class NoteTemp
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public DateTime CreatedOnUtc { get; set; }
    public DateTime LastModifiedOnUtc { get; set; }
    public int IdUser { get; set; }

    public Note Convert()
    {
        return Note
            .Empty()
            .Load(
                id: NoteId.Create(Id),
                title: Title,
                content: Content,
                createdOnUtc: CreatedOnUtc,
                lastModifiedOnUtc: LastModifiedOnUtc,
                tags: new List<Tag>(),
                userId: UserId.Create(IdUser));
    }
}