using Newtonsoft.Json;
using Studweb.Domain.Common.Interfaces;
using Studweb.Domain.Entities.Enums;
using Studweb.Domain.Entities.ValueObjects;
using Studweb.Domain.Primitives;

namespace Studweb.Domain.Entities;

public class ResetPasswordToken : Entity<ResetPasswordTokenId>, IToken
{
    public Guid Value { get; }
    public DateTime CreatedOnUtc { get; }
    public DateTime ExpiresOnUtc { get; }
    public TokenType Type { get; }
    
    [JsonIgnore]
    public User User { get; }

    [JsonConstructor]
    private ResetPasswordToken(User user, ResetPasswordTokenId id = default) : base(id)
    {
        User = user;
        Value = Guid.NewGuid();
        CreatedOnUtc = DateTime.UtcNow;
        ExpiresOnUtc = DateTime.UtcNow.AddDays(1);
        Type = TokenType.ResetPasswordToken;
    }

    public static ResetPasswordToken Create(User user) => new ResetPasswordToken(user);
    
    public bool Verify()
    {
        if (ExpiresOnUtc.CompareTo(DateTime.UtcNow) == 1)
        {
            return false;
        }

        return true;
    }
}