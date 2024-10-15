using ErrorOr;

namespace Studweb.Domain.Common.Errors;

public static partial class Errors
{
    public static class User
    {
        public static Error DuplicateEmail = Error.Conflict(
            code: "User.DuplicateEmail",
            description: "User with that email exists");

        public static Error UserNotFound = Error.NotFound(
            code: "User.UserNotFound",
            description: "User with that email not found");
    }
}