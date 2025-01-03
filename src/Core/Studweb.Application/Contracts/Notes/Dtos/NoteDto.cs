using Studweb.Domain.Aggregates.Notes.ValueObjects;

namespace Studweb.Application.Contracts.Notes.Dtos;

public record NoteDto(
    int Id,
    string Title,
    string Content,
    DateTime CreatedOnUtc,
    DateTime LastModifiedOnUtc
);