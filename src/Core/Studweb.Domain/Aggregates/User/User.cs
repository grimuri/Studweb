using System.Security.Cryptography;
using Studweb.Domain.Entities.ValueObjects;
using Studweb.Domain.Primitives;

namespace Studweb.Domain.Entities;

public class User : AggregateRoot<UserId>
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public DateTime? Birthday { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? VerifiedAt { get; set; }
    public string? VerificationToken { get; set; }
    public DateTime? VerificationTokenExpires { get; set; }
    public string? ResetPasswordToken { get; set; }
    public DateTime? ResetPasswordTokenExpires { get; set; }
    public DateTime? BanTime { get; set; }

    public Role Role { get; set; }

    private User(UserId id, string firstName, string lastName, string email, string password, DateTime? birthday, Role role) : base(id)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Password = password;
        Birthday = birthday;
        CreatedAt = DateTime.Now;
        VerificationToken = RandomNumberGenerator.GetBytes(64).ToString();
        VerificationTokenExpires = DateTime.Now.AddDays(3);
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
        return new(
            UserId.Create(-1),
            firstName,
            lastName,
            email,
            password,
            birthday,
            role);
    }
}















