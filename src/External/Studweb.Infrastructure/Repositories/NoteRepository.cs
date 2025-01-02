using Dapper;
using Studweb.Application.Persistance;
using Studweb.Domain.Aggregates.Notes;
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

    public Task<Note?> GetByTitleAsync(string title, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
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
        var tags = new List<(int Id, string Name)>();
        foreach (var tag in note.Tags)
        {
            var tagId = await _tagRepository.GetIdTagByNameAsync(tag.Value);

            if (tagId is null)
            {
                tagId = await _tagRepository.CreateAsync(tag);
            }

            tags.Add((Convert.ToInt32(tagId), tag.Value));
        }

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
        const string tagSql = @"INSERT INTO Notes_Tags VALUES (@NoteId, @TagId)";

        foreach (var tag in tags)
        {
            await connection.ExecuteScalarAsync(tagSql, new { NoteId = noteId, TagId = tag.Id });
        }

        return noteId;
    }
}