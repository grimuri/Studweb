using Studweb.Application.Common.Messaging;
using Studweb.Application.Contracts.Notes.Responses;

namespace Studweb.Application.Features.Notes.Commands.DeleteNote;

public sealed record DeleteNoteCommand(int Id) : ICommand<DeleteNoteResponse>;