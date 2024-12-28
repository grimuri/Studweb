using Studweb.Domain.Aggregates.Users.Entities;

namespace Studweb.IntegrationTests.TestUtils.DataToSeed;

public static class RoleToSeed
{
    public static Role GivenRoleUser()
    {
        return Role.Create("User");
    }
}