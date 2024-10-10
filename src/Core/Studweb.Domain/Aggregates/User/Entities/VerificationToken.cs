using Newtonsoft.Json;
using Studweb.Domain.Common.Interfaces;
using Studweb.Domain.Entities.Enums;
using Studweb.Domain.Entities.ValueObjects;
using Studweb.Domain.Primitives;

namespace Studweb.Domain.Entities;

public class VerificationToken : Entity<VerificationTokenId>, IToken
{
    public Guid Value { get; }
    public DateTime CreatedOnUtc { get; }
    public DateTime ExpiresOnUtc { get; }
    public TokenType Type { get; }

    [JsonConstructor]
    private VerificationToken(VerificationTokenId id = default) : base(id)
    {
        Value = Guid.NewGuid();
        CreatedOnUtc = DateTime.UtcNow;
        ExpiresOnUtc = DateTime.UtcNow.AddDays(1);
        Type = TokenType.VerificationToken;
    }

    public static VerificationToken Create() => new VerificationToken();
    
    public bool Verify()
    {
        if (ExpiresOnUtc.CompareTo(DateTime.UtcNow) == 1)
        {
            return false;
        }

        return true;
    }
}