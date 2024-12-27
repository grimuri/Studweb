﻿using Studweb.Domain.Aggregates.Note.ValueObjects;

namespace Studweb.IntegrationTests.TestUtils.Constants;

public static partial class Constants
{
    public static class AddNote
    {
        public const string Title = "Title";
        public const string Content = "Content";

        public static readonly List<Tag> Tags = new()
        {
            Tag.Create("Tag1"),
            Tag.Create("Tag2"),
            Tag.Create("Tag3")
        };
    }
}