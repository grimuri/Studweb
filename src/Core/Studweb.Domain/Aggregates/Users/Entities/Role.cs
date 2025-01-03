using Newtonsoft.Json;
using Studweb.Domain.Aggregates.Users.ValueObjects;
using Studweb.Domain.Primitives;

namespace Studweb.Domain.Aggregates.Users.Entities;

public sealed class Role : Entity<RoleId>
{
    public string Name { get; set; }
    
    [JsonConstructor]
    private Role(RoleId id, string name) : base(id)
    {
        Name = name;
    }

    public static Role Create(string name, RoleId? id = null)
    {
        if (id is not null)
        {
            return new Role(
                RoleId.Create(id.Value),
                name);
        }
        return new Role(
            RoleId.Create(-1),
            name);
        
    }
}