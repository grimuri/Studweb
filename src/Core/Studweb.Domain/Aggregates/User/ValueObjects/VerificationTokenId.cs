using Newtonsoft.Json;
using Studweb.Domain.Primitives;

namespace Studweb.Domain.Entities.ValueObjects;

public class VerificationTokenId : ValueObject
{
    public int Value { get; }

    [JsonConstructor]
    private VerificationTokenId(int value = default)
    {
        Value = value;
    }

    public static VerificationTokenId Create() => new VerificationTokenId();

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}