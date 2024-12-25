using Newtonsoft.Json;
using Studweb.Domain.Primitives;

namespace Studweb.Domain.Aggregates.User.ValueObjects;

public sealed class RoleId : ValueObject
{
    public int Value { get; }

    [JsonConstructor]
    private RoleId(int value)
    {
        Value = value;
    }

    public static RoleId Create(int id) => new RoleId(id);
    
    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}