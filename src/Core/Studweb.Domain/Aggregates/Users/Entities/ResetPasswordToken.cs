using Newtonsoft.Json;
using Studweb.Domain.Aggregates.Users.Enums;
using Studweb.Domain.Aggregates.Users.ValueObjects;
using Studweb.Domain.Common.Interfaces;
using Studweb.Domain.Primitives;

namespace Studweb.Domain.Aggregates.Users.Entities;

public class ResetPasswordToken : Entity<ResetPasswordTokenId>, IToken
{
    public Guid Value { get; private set; }
    public DateTime CreatedOnUtc { get; private set; }
    public DateTime ExpiresOnUtc { get; private set; }
    public TokenType Type { get; private set; }

    [JsonConstructor]
    private ResetPasswordToken(ResetPasswordTokenId id) : base(id)
    {
        Value = Guid.NewGuid();
        CreatedOnUtc = DateTime.UtcNow;
        ExpiresOnUtc = DateTime.UtcNow.AddDays(1);
        Type = TokenType.ResetPasswordToken;
    }

    public ResetPasswordToken Load(
        ResetPasswordTokenId id,
        Guid value,
        DateTime createdOnUtc,
        DateTime expiresOnUtc,
        TokenType tokenType
    )
    {
        Id = id;
        Value = value;
        CreatedOnUtc = createdOnUtc;
        ExpiresOnUtc = expiresOnUtc;
        Type = tokenType;
        return this;
    }

    public static ResetPasswordToken Create(ResetPasswordTokenId id = default) => new ResetPasswordToken(id);
    
    public bool Verify()
    {
        if (ExpiresOnUtc.CompareTo(DateTime.UtcNow) == 1)
        {
            return false;
        }

        return true;
    }
}