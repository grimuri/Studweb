using Studweb.Domain.Aggregates.Note.ValueObjects;

namespace Studweb.Application.Persistance;

public interface ITagRepository
{
    Task<IEnumerable<Tag>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<int?> GetIdTagByNameAsync(string name, CancellationToken cancellationToken = default);
    Task<Tag?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
    Task<int> CreateAsync(Tag tag, CancellationToken cancellationToken = default);
}