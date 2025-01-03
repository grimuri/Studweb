using Studweb.Domain.Aggregates.Notes;
using Studweb.Domain.Aggregates.Notes.ValueObjects;
using Studweb.Domain.Aggregates.Users.ValueObjects;

namespace Studweb.Infrastructure.Utils.TempClasses;

public class NoteWithTagsTemp
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public DateTime CreatedOnUtc { get; set; }
    public DateTime LastModifiedOnUtc { get; set; }
    public int IdUser { get; set; }
    public string? TagName { get; set; }

    public static Note? Convert(IEnumerable<NoteWithTagsTemp> tempNotes)
    {
        var notes = tempNotes
            .GroupBy(x =>
                new
                {
                    x.Id,
                    x.Title,
                    x.Content,
                    x.CreatedOnUtc,
                    x.LastModifiedOnUtc,
                    x.IdUser
                })
            .Select(x =>
                Note
                    .Empty()
                    .Load(
                        id: NoteId.Create(x.Key.Id),
                        title: x.Key.Title,
                        content: x.Key.Content,
                        createdOnUtc: x.Key.CreatedOnUtc,
                        lastModifiedOnUtc: x.Key.LastModifiedOnUtc,
                        tags: x
                            .Select(r => r.TagName)
                            .Where(r => r != null)
                            .Select(r => Tag.Create(r))
                            .ToList(),
                        userId: UserId.Create(x.Key.IdUser))
            )
            .ToList();

        return notes.FirstOrDefault();
    }
}