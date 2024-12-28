using Studweb.Domain.Aggregates.Notes;

namespace Studweb.Application.Contracts.Notes;

public record GetAllNotesResponse(List<Note> notes);