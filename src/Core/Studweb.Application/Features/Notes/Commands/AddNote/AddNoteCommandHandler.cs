using ErrorOr;
using Studweb.Application.Abstractions.Messaging;
using Studweb.Application.Contracts.Notes;
using Studweb.Application.Persistance;
using Studweb.Application.Utils;
using Studweb.Domain.Aggregates.Notes;
using Studweb.Domain.Aggregates.Notes.ValueObjects;
using Studweb.Domain.Aggregates.Users.ValueObjects;
using Studweb.Domain.Common.Errors;

namespace Studweb.Application.Features.Notes.Commands.AddNote;

public class AddNoteCommandHandler : ICommandHandler<AddNoteCommand, AddNoteResponse>
{
    private readonly INoteRepository _noteRepository;
    private readonly IUserContext _userContext;

    public AddNoteCommandHandler(
        INoteRepository noteRepository, 
        IUserContext userContext)
    {
        _noteRepository = noteRepository;
        _userContext = userContext;
    }

    public async Task<ErrorOr<AddNoteResponse>> Handle(AddNoteCommand request, CancellationToken cancellationToken)
    {
        var userId = _userContext.UserId;
        
        if (userId is null)
        {
            return Errors.User.UserNotAuthenticated;
        }
        
        var note = Note.Create(
            NoteId.Create(), 
            request.Title,
            request.Content,
            request.Tags,
            UserId.Create(userId ?? 0)
        );
        
        int noteId = await _noteRepository.CreateAsync(note);

        return new AddNoteResponse(noteId, "Successfully added note!");
    }
}