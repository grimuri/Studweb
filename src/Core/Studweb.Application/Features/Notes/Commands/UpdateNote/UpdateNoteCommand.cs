using Studweb.Application.Common.Messaging;
using Studweb.Application.Contracts.Notes.Responses;
using Studweb.Domain.Aggregates.Notes;
using Studweb.Domain.Aggregates.Notes.ValueObjects;

namespace Studweb.Application.Features.Notes.Commands.UpdateNote;

public record UpdateNoteCommand(
    int Id,
    string Title,
    string Content,
    List<Tag> Tags) : ICommand<UpdateNoteResponse>;