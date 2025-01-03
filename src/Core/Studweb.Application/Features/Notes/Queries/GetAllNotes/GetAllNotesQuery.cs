using Studweb.Application.Abstractions.Messaging;
using Studweb.Application.Contracts.Notes;
using Studweb.Application.Contracts.Notes.Responses;

namespace Studweb.Application.Features.Notes.Queries.GetAllNotes;

public sealed record GetAllNotesQuery() : IQuery<GetAllNotesResponse>;