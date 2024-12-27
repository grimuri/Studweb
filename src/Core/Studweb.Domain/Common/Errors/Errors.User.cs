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

        public static Error UserNotVerified = Error.Conflict(
            code: "User.UserNotVerified",
            description: "User is not verified");

        public static Error IncorrectData = Error.Conflict(
            code: "User.IncorrectData",
            description: "Login or password is incorrect");

        public static Error UserNotAuthenticated = Error.Forbidden(
            code: "User.UserNotAuthenticated",
            description: "User is not authenticated");
    }
}