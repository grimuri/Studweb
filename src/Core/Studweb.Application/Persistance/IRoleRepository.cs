using Studweb.Domain.Entities;

namespace Studweb.Application.Persistance;

public interface IRoleRepository
{
    Task<IEnumerable<Role>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Role?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<Role?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
    Task<int> AddAsync(Role role, CancellationToken cancellationToken = default);
    Task<Role> EditAsync(int id, string name, CancellationToken cancellationToken = default);
}