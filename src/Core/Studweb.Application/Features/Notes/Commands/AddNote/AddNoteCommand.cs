using Studweb.Application.Abstractions.Messaging;
using Studweb.Application.Contracts.Note;
using Studweb.Domain.Aggregates.Note.ValueObjects;

namespace Studweb.Application.Features.Notes.Commands.AddNote;

public record AddNoteCommand(
    string Title,
    string Content,
    List<Tag> Tags) : ICommand<AddNoteResponse>;