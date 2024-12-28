using Newtonsoft.Json;
using Studweb.Domain.Aggregates.Users.Enums;
using Studweb.Domain.Aggregates.Users.ValueObjects;
using Studweb.Domain.Common.Interfaces;
using Studweb.Domain.Primitives;

namespace Studweb.Domain.Aggregates.Users.Entities;

public class VerificationToken : Entity<VerificationTokenId>, IToken
{
    public Guid Value { get; private set; }
    public DateTime CreatedOnUtc { get; private set; }
    public DateTime ExpiresOnUtc { get; private set; }
    public TokenType Type { get; private set; }

    [JsonConstructor]
    private VerificationToken(VerificationTokenId id = default) : base(id)
    {
        Value = Guid.NewGuid();
        CreatedOnUtc = DateTime.UtcNow;
        ExpiresOnUtc = DateTime.UtcNow.AddDays(1);
        Type = TokenType.VerificationToken;
    }

    public static VerificationToken Create(VerificationTokenId id = default) => new VerificationToken(id);

    public VerificationToken Load(
        VerificationTokenId id,
        Guid value,
        DateTime createdOnUtc,
        DateTime expiresOnUtc,
        TokenType tokenType)
    {
        Id = id;
        Value = value;
        CreatedOnUtc = createdOnUtc;
        ExpiresOnUtc = expiresOnUtc;
        Type = tokenType;
        return this;
    }

    public bool Verify()
    {
        if (ExpiresOnUtc.CompareTo(DateTime.UtcNow) == 1)
        {
            return false;
        }

        return true;
    }
    
}