using Studweb.Domain.Entities;
using Studweb.Domain.Entities.ValueObjects;

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