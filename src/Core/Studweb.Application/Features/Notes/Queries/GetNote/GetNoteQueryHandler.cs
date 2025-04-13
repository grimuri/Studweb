using ErrorOr;
using Studweb.Application.Common.Messaging;
using Studweb.Application.Contracts.Notes;
using Studweb.Application.Contracts.Notes.Dtos;
using Studweb.Application.Contracts.Notes.Responses;
using Studweb.Application.Persistance;
using Studweb.Application.Utils;
using Studweb.Domain.Common.Errors;

namespace Studweb.Application.Features.Notes.Queries.GetNote;

public class GetNoteQueryHandler : IQueryHandler<GetNoteQuery, GetNoteResponse>
{
    private readonly IUserContext _userContext;
    private readonly INoteRepository _noteRepository;

    public GetNoteQueryHandler(
        IUserContext userContext,
        INoteRepository noteRepository)
    {
        _userContext = userContext;
        _noteRepository = noteRepository;
    }

    public async Task<ErrorOr<GetNoteResponse>> Handle(GetNoteQuery request, CancellationToken cancellationToken)
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

        return new GetNoteResponse(
            note.Id.Value,
            note.Title,
            note.Content,
            note.CreatedOnUtc,
            note.LastModifiedOnUtc,
            note.Tags
            );
    }
}