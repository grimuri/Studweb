using Studweb.Application.Persistance;
using Studweb.Domain.Entities;

namespace Studweb.Infrastructure.Repositories;

public class RoleRepository : IRoleRepository
{
    private List<Role> _roles = new List<Role>()
    {
        new()
        {
            Id = 0,
            Name = "Admin",
        },
        new()
        {
            Id = 1,
            Name = "Moderator",
        }
    };
    public async Task<IEnumerable<Role>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return _roles;
    }

    public async Task<Role> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var role = _roles.FirstOrDefault(x => x.Id == id);
        return role;
    }
}