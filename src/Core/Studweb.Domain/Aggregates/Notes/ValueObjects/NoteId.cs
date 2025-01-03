using Newtonsoft.Json;
using Studweb.Domain.Primitives;

namespace Studweb.Domain.Aggregates.Notes.ValueObjects;

public sealed class NoteId : ValueObject
{
    public int Value { get; }

    [JsonConstructor]
    private NoteId(int value = default)
    {
        Value = value;
    }

    public static NoteId Create(int id = default) => new NoteId(id);
    
    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}