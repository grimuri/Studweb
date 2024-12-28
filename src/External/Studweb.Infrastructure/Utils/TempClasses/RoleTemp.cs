using Studweb.Domain.Aggregates.Users.Entities;
using Studweb.Domain.Aggregates.Users.ValueObjects;

namespace Studweb.Infrastructure.Utils.TempClasses;

public class RoleTemp
{
    public int Id { get; set; }

    public string Name { get; set; }

    public Role Convert()
    {
        return Role.Create(Name, RoleId.Create(Id));
    }
}