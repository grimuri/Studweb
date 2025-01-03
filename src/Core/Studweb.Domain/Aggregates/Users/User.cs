using Newtonsoft.Json;
using Studweb.Domain.Aggregates.Users.Entities;
using Studweb.Domain.Aggregates.Users.ValueObjects;
using Studweb.Domain.DomainEvents;
using Studweb.Domain.Primitives;

namespace Studweb.Domain.Aggregates.Users;

public sealed class User : AggregateRoot<UserId>
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public DateTime Birthday { get; set; }
    public DateTime CreatedOnUtc { get; private set; }
    
    // TODO: Function to verify user
    public DateTime? VerifiedOnUtc { get; private set; } = DateTime.UtcNow;
    
    public DateTime? LastModifiedPasswordOnUtc { get; private set; }
    public VerificationToken? VerificationToken { get; private set; }
    
    // TODO: Function to generate reset password token
    public ResetPasswordToken? ResetPasswordToken { get; private set; }
    public DateTime? BanTime { get; set; }
    public Role Role { get; set; }

    private User(UserId id = default) : base(id)
    {
        
    }

    [JsonConstructor]
    private User(
        UserId id,
        string firstName,
        string lastName,
        string email, 
        string password, 
        DateTime birthday, 
        Role role
        ) : base(id)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Password = password;
        Birthday = birthday;
        CreatedOnUtc = DateTime.UtcNow;
        VerificationToken = VerificationToken.Create(VerificationTokenId.Create(-1));
        Role = role;
    }

    public static User Create(
        UserId? userId,
        string firstName,
        string lastName,
        string email,
        string password,
        DateTime birthday,
        Role role
    )
    {
        if (userId is null)
        {
            userId = UserId.Create(-1);
        }
        
        var user = new User(
            userId,
            firstName,
            lastName,
            email,
            password,
            birthday,
            role);
        
        user.RaiseDomainEvent(new UserRegistered(user));

        return user;
    }

    public User Load(
        UserId id,
        string firstName,
        string lastName,
        string email,
        string password,
        DateTime birthday,
        DateTime createdOnUtc,
        DateTime? verifiedOnUtc,
        DateTime? lastModifiedPasswordOnUtc,
        DateTime? banTime,
        Role role,
        VerificationToken verificationToken,
        ResetPasswordToken resetPasswordToken
    )
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Password = password;
        Birthday = birthday;
        CreatedOnUtc = createdOnUtc;
        VerifiedOnUtc = verifiedOnUtc;
        LastModifiedPasswordOnUtc = lastModifiedPasswordOnUtc;
        BanTime = banTime;
        Role = role;
        VerificationToken = verificationToken;
        ResetPasswordToken = resetPasswordToken;
        return this;
    }

    public static User Empty() => new User();

}















