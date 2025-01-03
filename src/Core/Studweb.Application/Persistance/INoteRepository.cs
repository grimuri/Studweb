using Studweb.Domain.Aggregates.Notes;

namespace Studweb.Application.Persistance;

public interface INoteRepository
{
    Task<Note?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Note>> GetAllNotesAsync(int userId, CancellationToken cancellationToken = default);
    Task<int> CreateAsync(Note note, CancellationToken cancellationToken = default);
    Task<Note> UpdateAsync(Note note, CancellationToken cancellationToken = default);
    Task<int> DeleteAsync(int id, CancellationToken cancellationToken = default);
}