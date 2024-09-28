using Newtonsoft.Json;
using Studweb.Domain.Primitives;

namespace Studweb.Domain.Entities.ValueObjects;

public class ResetPasswordTokenId : ValueObject
{
    public int Value { get; }

    [JsonConstructor]
    private ResetPasswordTokenId(int value = default)
    {
        Value = value;
    }

    public static ResetPasswordTokenId Create() => new ResetPasswordTokenId();
    
    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}