using Studweb.Domain.Aggregates.User;
using Studweb.Domain.Aggregates.User.Entities;
using Studweb.Domain.Aggregates.User.ValueObjects;

namespace Studweb.Application.UnitTests.Features.Users.Commands.RegisterUser.TestUtils;

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
}