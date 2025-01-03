using Studweb.Application.Abstractions.Messaging;
using Studweb.Application.Contracts.Notes.Responses;

namespace Studweb.Application.Features.Notes.Commands.DeleteNote;

public sealed record DeleteNoteCommand(int Id) : ICommand<DeleteNoteResponse>;