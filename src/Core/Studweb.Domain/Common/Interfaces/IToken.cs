using Studweb.Domain.Aggregates.Users.Enums;

namespace Studweb.Domain.Common.Interfaces;

public interface IToken
{
    public Guid Value { get; }
    public DateTime CreatedOnUtc { get; }
    public DateTime ExpiresOnUtc { get; }
    public TokenType Type { get; }

    public bool Verify();
}