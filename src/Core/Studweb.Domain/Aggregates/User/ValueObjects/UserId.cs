using Studweb.Domain.Primitives;

namespace Studweb.Domain.Entities.ValueObjects;


public class UserId : ValueObject
{
    public int Value { get; }

    private UserId(int value)
    {
        Value = value;
    }

    public static UserId Create(int id) => new UserId(id);
    
    public override IEnumerable<object> GetEqualityComponents()
    {
        throw new NotImplementedException();
    }
}