using Newtonsoft.Json;
using Studweb.Domain.Primitives;

namespace Studweb.Domain.Aggregates.Notes.ValueObjects;

// TODO: Move to domain layer

public sealed class Tag : ValueObject
{
    public string Value { get; }
    
    [JsonConstructor]
    private Tag(string value)
    {
        Value = value;
    }

    public static Tag Create(string value) => new Tag(value);
    
    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}