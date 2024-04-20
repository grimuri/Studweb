using Studweb.Application.Persistance;
using Studweb.Domain.Entities;

namespace Studweb.Infrastructure.Repositories;

public class RoleRepository : IRoleRepository
{
    private List<Role> _roles = new List<Role>()
    {
        new()
        {
            Name = "Admin",
        },
        new()
        {
            Name = "Moderator",
        }
    };
    public async Task<IEnumerable<Role>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return _roles;
    }
}