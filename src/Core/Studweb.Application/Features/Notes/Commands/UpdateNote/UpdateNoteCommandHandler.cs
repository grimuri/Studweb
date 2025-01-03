using ErrorOr;
using Studweb.Application.Abstractions.Messaging;
using Studweb.Application.Contracts.Notes.Responses;
using Studweb.Application.Persistance;
using Studweb.Application.Utils;
using Studweb.Domain.Common.Errors;

namespace Studweb.Application.Features.Notes.Commands.UpdateNote;

public class UpdateNoteCommandHandler : ICommandHandler<UpdateNoteCommand, UpdateNoteResponse>
{
    private readonly IUserContext _userContext;
    private readonly INoteRepository _noteRepository;

    public UpdateNoteCommandHandler(
        IUserContext userContext,
        INoteRepository noteRepository)
    {
        _userContext = userContext;
        _noteRepository = noteRepository;
    }

    public async Task<ErrorOr<UpdateNoteResponse>> Handle(UpdateNoteCommand request, CancellationToken cancellationToken)
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

        var newNote = note.Update(
            request.Title,
            request.Content,
            request.Tags);

        await _noteRepository.UpdateAsync(newNote);

        return new UpdateNoteResponse(
            newNote.Id.Value,
            newNote.Title,
            newNote.Content,
            newNote.CreatedOnUtc,
            newNote.LastModifiedOnUtc,
            newNote.Tags);
    }
}