using Studweb.Domain.Aggregates.Notes;

namespace Studweb.Application.Persistance;

public interface INoteRepository
{
    Task<Note?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<Note?> GetByTitleAsync(string title, CancellationToken cancellationToken = default);
    Task<IEnumerable<Note>> GetAllNotes(int userId, CancellationToken cancellationToken = default);
    Task<int> CreateAsync(Note note, CancellationToken cancellationToken = default);
}