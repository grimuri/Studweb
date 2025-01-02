using Studweb.Application.Contracts.Notes.Dtos;
using Studweb.Domain.Aggregates.Notes;

namespace Studweb.Application.Contracts.Notes.Responses;

public record GetAllNotesResponse(List<NoteDto> Notes);