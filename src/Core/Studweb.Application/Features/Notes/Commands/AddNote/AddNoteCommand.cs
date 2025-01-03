using Studweb.Application.Abstractions.Messaging;
using Studweb.Application.Contracts.Notes;
using Studweb.Application.Contracts.Notes.Responses;
using Studweb.Domain.Aggregates.Notes.ValueObjects;

namespace Studweb.Application.Features.Notes.Commands.AddNote;

public record AddNoteCommand(
    string Title,
    string Content,
    List<Tag> Tags) : ICommand<AddNoteResponse>;