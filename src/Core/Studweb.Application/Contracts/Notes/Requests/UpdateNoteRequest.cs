using Studweb.Domain.Aggregates.Notes.ValueObjects;

namespace Studweb.Application.Contracts.Notes.Requests;

public record UpdateNoteRequest(
    string Title,
    string Content,
    List<Tag> Tags);