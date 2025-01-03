using Studweb.Domain.Aggregates.Notes.ValueObjects;

namespace Studweb.IntegrationTests.TestUtils.Constants;

public static partial class Constants
{
    public static class UpdateNote
    {
        public const int Id = 1;
        public const string Title = "NewTitle";
        public const string Content = "NewContent";

        public static readonly List<Tag> Tags = new()
        {
            Tag.Create("Tag12"),
            Tag.Create("Tag22"),
            Tag.Create("Tag32")
        };
    }
}