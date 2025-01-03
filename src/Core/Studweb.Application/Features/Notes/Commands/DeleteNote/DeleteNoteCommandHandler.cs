using ErrorOr;
using Studweb.Application.Abstractions.Messaging;
using Studweb.Application.Contracts.Notes.Responses;
using Studweb.Application.Persistance;
using Studweb.Application.Utils;
using Studweb.Domain.Common.Errors;

namespace Studweb.Application.Features.Notes.Commands.DeleteNote;

public sealed class DeleteNoteCommandHandler 
    : ICommandHandler<DeleteNoteCommand, DeleteNoteResponse>
{
    private readonly IUserContext _userContext;
    private readonly INoteRepository _noteRepository;

    public DeleteNoteCommandHandler(IUserContext userContext, INoteRepository noteRepository)
    {
        _userContext = userContext;
        _noteRepository = noteRepository;
    }

    public async Task<ErrorOr<DeleteNoteResponse>> Handle(DeleteNoteCommand request, CancellationToken cancellationToken)
    {
        var userId = _userContext.UserId;

        if (userId is null)
        {
            return Errors.User.UserNotAuthenticated;
        }

        var note = await _noteRepository.GetByIdAsync(request.Id);

        if (note is null)
        {
            return Errors.Note.NotFound;
        }

        if (note.UserId.Value != userId.Value)
        {
            return Errors.Note.AccessDenied;
        }

        var deletedNotes = await _noteRepository.DeleteAsync(request.Id);

        if (deletedNotes == 0)
        {
            return Errors.Note.CannotDeleteNote;
        }

        return new DeleteNoteResponse(
            request.Id,
            "Note successfully deleted");
    }
}