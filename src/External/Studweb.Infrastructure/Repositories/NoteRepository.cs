using System.Data;
using Dapper;
using Studweb.Application.Persistance;
using Studweb.Domain.Aggregates.Notes;
using Studweb.Domain.Aggregates.Notes.ValueObjects;
using Studweb.Infrastructure.Persistance;
using Studweb.Infrastructure.Utils.TempClasses;

namespace Studweb.Infrastructure.Repositories;

public sealed class NoteRepository : INoteRepository
{
    private readonly IDbContext _dbContext;
    private readonly ITagRepository _tagRepository;

    public NoteRepository(IDbContext dbContext,
        ITagRepository tagRepository)
    {
        _dbContext = dbContext;
        _tagRepository = tagRepository;
    }

    public async Task<Note?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var connection = _dbContext.Connection;

        const string sql = @"SELECT Notes.Id, Notes.Title, Notes.Content, Notes.CreatedOnUtc, Notes.LastModifiedOnUtc, 
                                Notes.UserId AS IdUser, T.Name AS TagName
                         FROM Notes
                         LEFT JOIN Notes_Tags NT on Notes.Id = NT.NoteId
                         LEFT JOIN Tags T on T.Id = NT.TagId
                         WHERE Notes.Id = @Id";

        var tempNotes = await connection.QueryAsync<NoteWithTagsTemp>(sql, new { Id = id });

        var note = NoteWithTagsTemp.Convert(tempNotes);

        return note;
    }

    public async Task<IEnumerable<Note>> GetAllNotesAsync(int userId, CancellationToken cancellationToken = default)
    {
        var connection = _dbContext.Connection;

        const string sql = @"SELECT Id, Title, Content, CreatedOnUtc, LastModifiedOnUtc, 
                                UserId AS IdUser FROM Notes WHERE UserId = @UserId";

        var notesTemp = await connection
            .QueryAsync<NoteTemp>(sql, new { UserId = userId });

        var notes = notesTemp.Select(x => x.Convert());

        return notes;
    }

    public async Task<int> CreateAsync(Note note, CancellationToken cancellationToken = default)
    {
        var connection = _dbContext.Connection;

        // Add tags
        var tags = await AddTagAsync(note.Tags);

        // Add note
        const string noteSql = @"INSERT INTO Notes 
                                (Title, Content, CreatedOnUtc, LastModifiedOnUtc, UserId) 
                                OUTPUT Inserted.Id
                                VALUES
                                (@Title, @Content, @CreatedOnUtc, @LastModifiedOnUtc, @UserId)";

        var parameters = new
        {
            Title = note.Title,
            Content = note.Content,
            CreatedOnUtc = note.CreatedOnUtc,
            LastModifiedOnUtc = note.LastModifiedOnUtc,
            UserId = note.UserId.Value
        };

        var noteId = await connection.ExecuteScalarAsync<int>(noteSql, parameters);

        // Connect note with tags
        await ConnectNoteWithTagsAsync(connection, noteId, tags);

        return noteId;
    }

    public async Task<Note> UpdateAsync(Note note, CancellationToken cancellationToken = default)
    {
        var connection = _dbContext.Connection;

        // Update Note
        const string noteSql = @"UPDATE Notes 
                                SET Title = @Title, Content = @Content, LastModifiedOnUtc = @LastModifiedOnUtc
                                WHERE Id = @Id";

        var noteParameters = new
        {
            Title = note.Title,
            Content = note.Content,
            LastModifiedOnUtc = note.LastModifiedOnUtc,
            Id = note.Id.Value
        };

        await connection.ExecuteScalarAsync(noteSql, noteParameters);
        
        // Get unused tags
        var tags = await AddTagAsync(note.Tags);

        const string tagSql = @"SELECT T.* 
                                    FROM Tags T LEFT JOIN Notes_Tags NT ON T.Id = NT.TagId
                                    WHERE NT.NoteId = @NoteId";

        var savedTags = await connection.QueryAsync<TagTemp>(tagSql, new { NoteId = note.Id.Value });

        var toDeleteTags = savedTags
            .Where(x => !tags.Any(z => z.Id == x.Id))
            .ToList();

        // Unconnect tags from note
        const string deleteTagsInNoteSql = @"DELETE FROM Notes_Tags WHERE NoteId = @NoteId AND TagId IN @Tags";

        await connection.ExecuteScalarAsync(deleteTagsInNoteSql, new
        {
            NoteId = note.Id.Value, 
            Tags = toDeleteTags.Select(x => x.Id).ToArray()
        });

        // Delete unused tags
        await _tagRepository.DeleteRangeAsync(toDeleteTags.Select(x => x.Id));

        await ConnectNoteWithTagsAsync(connection, note.Id.Value, tags);

        return note;
    }

    public async Task<int> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var connection = _dbContext.Connection;

        // Get connected tags
        const string connectedTagsSql =
            @"SELECT NT.* 
            FROM Notes_Tags NT LEFT JOIN Notes N on N.Id = NT.NoteId 
            WHERE NT.NoteId = @NoteId";

        var connectedTags = await connection
            .QueryAsync<(int NoteId, int TagId)>(connectedTagsSql, new { NoteId = id });

        // Unconnect tags from note
        const string deleteConnectionSql = @"DELETE FROM Notes_Tags WHERE NoteId = @NoteId";

        await connection.ExecuteAsync(deleteConnectionSql, new { NoteId = id });
        
        // Delete note
        const string deleteNoteSql = @"DELETE FROM Notes WHERE Id = @NoteId";

        var affectedRows = await connection.ExecuteAsync(deleteNoteSql, new { NoteId = id });
        
        // Delete connected tags
        await _tagRepository.DeleteRangeAsync(connectedTags.Select(x => x.TagId));

        return affectedRows;
    }

    private async Task<IEnumerable<TagTemp>> AddTagAsync(List<Tag> listOfTags)
    {
        var tags = new List<TagTemp>();
        foreach (var tag in listOfTags)
        {
            var tagId = await _tagRepository.GetIdTagByNameAsync(tag.Value);

            if (tagId is null)
            {
                tagId = await _tagRepository.CreateAsync(tag);
            }

            tags.Add(new TagTemp()
            {
                Id = tagId.Value,
                Name = tag.Value
            });
        }

        return tags;
    }

    private async Task ConnectNoteWithTagsAsync(IDbConnection connection, int noteId, IEnumerable<TagTemp> tags)
    {
        // Get connected tags with note
        const string connectedTagsSql = @"SELECT T.* FROM Tags T LEFT JOIN Notes_Tags NT on T.Id = NT.TagId WHERE NoteId = @NoteId";

        var connectedTags = await connection.QueryAsync<TagTemp>(connectedTagsSql, new { NoteId = noteId });
        
        // Connect new tags with note
        const string tagSql = @"INSERT INTO Notes_Tags VALUES (@NoteId, @TagId)";

        var request = tags
            .Select(x => new
        {
            NoteId = noteId,
            TagId = x.Id
        })
            .Where(x => !connectedTags.Any(z => z.Id == x.TagId));
        
        await connection.ExecuteAsync(tagSql, request.ToList());
    }
}