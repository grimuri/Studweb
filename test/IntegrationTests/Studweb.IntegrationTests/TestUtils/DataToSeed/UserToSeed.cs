using Studweb.Domain.Aggregates.User;
using Studweb.Domain.Aggregates.User.Entities;
using Studweb.Domain.Aggregates.User.ValueObjects;

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