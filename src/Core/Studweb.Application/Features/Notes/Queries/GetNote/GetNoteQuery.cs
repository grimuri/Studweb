using Studweb.Application.Abstractions.Messaging;
using Studweb.Application.Contracts.Notes;
using Studweb.Application.Contracts.Notes.Responses;

namespace Studweb.Application.Features.Notes.Queries.GetNote;

public record GetNoteQuery(int Id) : IQuery<GetNoteResponse>;