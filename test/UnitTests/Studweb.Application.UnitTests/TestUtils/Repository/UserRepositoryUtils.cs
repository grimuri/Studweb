using System.Reflection;
using Studweb.Domain.Aggregates.Users;
using Studweb.Domain.Aggregates.Users.Entities;
using Studweb.Domain.Aggregates.Users.ValueObjects;

namespace Studweb.Application.UnitTests.TestUtils.Repository;

public static class UserRepositoryUtils
{
    public static User GetByEmailExampleUser()
    {
        return User.Create(
            UserId.Create(-1),
            "FirstName",
            "LastName",
            "user@gmail.com",
            "password123",
            DateTime.UtcNow.AddYears(-14),
            Role.Create("User")
        );
    }

    public static void SetFieldVerifiedOnUtcOnNull(User user)
    {
        var property = typeof(User)
            .GetProperty("VerifiedOnUtc", 
                BindingFlags.Instance | BindingFlags.Public);
        
        property.SetValue(user, null);
    }
    
    public static void SetFieldVerifiedOnUtcOnNotNull(User user)
    {
        var property = typeof(User)
            .GetProperty("VerifiedOnUtc", 
                BindingFlags.Instance | BindingFlags.Public);
        
        property.SetValue(user, DateTime.UtcNow);
    }
}