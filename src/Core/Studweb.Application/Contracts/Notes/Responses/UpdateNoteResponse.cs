using Studweb.Domain.Aggregates.Notes.ValueObjects;

namespace Studweb.Application.Contracts.Notes.Responses;

public record UpdateNoteResponse(
    int Id,
    string Title,
    string Content,
    DateTime CreatedOnUtc,
    DateTime LastModifiedOnUtc,
    List<Tag> Tags
);