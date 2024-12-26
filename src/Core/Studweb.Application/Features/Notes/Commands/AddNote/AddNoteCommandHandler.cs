using ErrorOr;
using Studweb.Application.Abstractions.Messaging;
using Studweb.Application.Contracts.Note;
using Studweb.Application.Persistance;
using Studweb.Application.Utils;
using Studweb.Domain.Aggregates.Note;
using Studweb.Domain.Aggregates.Note.ValueObjects;
using Studweb.Domain.Aggregates.User.ValueObjects;

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
        var note = Note.Create(
            NoteId.Create(), 
            request.Title,
            request.Content,
            request.Tags,
            UserId.Create(_userContext.UserId)
        );
        
        int noteId = await _noteRepository.CreateAsync(note);

        return new AddNoteResponse(noteId, "Successfully added note!");
    }
}