using Studweb.Domain.Aggregates.Users;
using Studweb.Domain.Aggregates.Users.Entities;
using Studweb.Domain.Aggregates.Users.ValueObjects;

namespace Studweb.IntegrationTests.TestUtils.DataToSeed;

public static class UserToSeed
{
    public static User GivenUser()
    {
        return User.Create(
            UserId.Create(-1),
            "FirstName",
            "LastName",
            "user@gmail.com",
            "Qwerty123",
            DateTime.UtcNow.AddYears(-15),
            Role.Create("User"));
    }
}