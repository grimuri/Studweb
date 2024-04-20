using ErrorOr;

namespace Studweb.Domain.Common.Errors;

public static partial class Errors
{
    public static class Role
    {
        public static Error NotFound = Error.NotFound(
            code: "Role.DoesntExist",
            description: "Role not found");
    }
}