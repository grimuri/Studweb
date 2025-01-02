using ErrorOr;

namespace Studweb.Domain.Common.Errors;

public static partial class Errors
{
    public static class Note
    {
        public static Error AccessDenied = Error.Forbidden(
            code: "Note.AccessDenied",
            description: "You do not have access to this note");

        public static Error NotFound = Error.NotFound(
            code: "Note.NotFound",
            description: "Note not found");
    }
}