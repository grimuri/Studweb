using Studweb.Domain.Aggregates.Notes.ValueObjects;

namespace Studweb.Application.UnitTests.TestUtils.Constants;

public static partial class Constants
{
    public static class UpdateNote
    {
        public const int Id = 1;
        public const string Title = "NewTitle";
        public const string Content = "NewContent";

        public static readonly List<Tag> Tags = new()
        {
            Tag.Create("Tag1"),
            Tag.Create("Tag2"),
            Tag.Create("Tag3")
        };
    }
}