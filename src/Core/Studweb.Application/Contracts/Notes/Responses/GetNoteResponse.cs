using Studweb.Application.Contracts.Notes.Dtos;
using Studweb.Domain.Aggregates.Notes;
using Studweb.Domain.Aggregates.Notes.ValueObjects;

namespace Studweb.Application.Contracts.Notes.Responses;

public record GetNoteResponse(
    int Id,
    string Title,
    string Content,
    DateTime CreatedOnUtc,
    DateTime LastModifiedOnUtc,
    List<Tag> Tags
    );