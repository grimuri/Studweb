using Studweb.Domain.Aggregates.Note;

namespace Studweb.Application.Persistance;

public interface INoteRepository
{
    Task<Note?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<Note?> GetByTitleAsync(string title, CancellationToken cancellationToken = default);
    Task<int> CreateAsync(Note note, CancellationToken cancellationToken = default);
}