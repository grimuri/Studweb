using System.Security.Cryptography;
using Newtonsoft.Json;
using Studweb.Domain.DomainEvents;
using Studweb.Domain.Entities.ValueObjects;
using Studweb.Domain.Primitives;

namespace Studweb.Domain.Entities;

public sealed class User : AggregateRoot<UserId>
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public DateTime? Birthday { get; set; }
    public DateTime CreatedOnUtc { get; }
    
    // TODO: Function to verify user
    public DateTime? VerifiedOnUtc { get; set; }
    
    public DateTime? LastModifiedPasswordOnUtc { get; set; }
    public VerificationToken? VerificationToken { get; }
    
    // TODO: Function to generate reset password token
    public ResetPasswordToken? ResetPasswordToken { get; private set; }
    public DateTime? BanTime { get; set; }
    public Role Role { get; set; }

    [JsonConstructor]
    private User(
        UserId id,
        string firstName,
        string lastName,
        string email, 
        string password, 
        DateTime? birthday, 
        Role role
        ) : base(id)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Password = password;
        Birthday = birthday;
        CreatedOnUtc = DateTime.UtcNow;
        VerificationToken = VerificationToken.Create();
        Role = role;
    }

    public static User Create(
        string firstName,
        string lastName,
        string email,
        string password,
        DateTime? birthday,
        Role role
    )
    {
        var user = new User(
            UserId.Create(-1), 
            firstName,
            lastName,
            email,
            password,
            birthday,
            role);
        
        user.RaiseDomainEvent(new UserRegistered(user));

        return user;
    }
}















