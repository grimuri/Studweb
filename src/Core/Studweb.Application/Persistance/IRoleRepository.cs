using Studweb.Domain.Entities;

namespace Studweb.Application.Persistance;

public interface IRoleRepository
{
    Task<IEnumerable<Role>> GetAllAsync(CancellationToken cancellationToken = default);
}