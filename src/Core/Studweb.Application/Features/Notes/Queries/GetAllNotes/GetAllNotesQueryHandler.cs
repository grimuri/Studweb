using ErrorOr;
using Studweb.Application.Abstractions.Messaging;
using Studweb.Application.Contracts.Notes;
using Studweb.Application.Persistance;
using Studweb.Application.Utils;
using Studweb.Domain.Common.Errors;

namespace Studweb.Application.Features.Notes.Queries.GetAllNotes;

public class GetAllNotesQueryHandler : IQueryHandler<GetAllNotesQuery, GetAllNotesResponse>
{
    private readonly IUserContext _userContext;
    private readonly INoteRepository _noteRepository;

    public GetAllNotesQueryHandler(
        IUserContext userContext, 
        INoteRepository noteRepository)
    {
        _userContext = userContext;
        _noteRepository = noteRepository;
    }

    public async Task<ErrorOr<GetAllNotesResponse>> Handle(GetAllNotesQuery request, CancellationToken cancellationToken)
    {
        var userId = _userContext.UserId;

        if (userId is null)
        {
            return Errors.User.UserNotAuthenticated;
        }
        
        var notes = await _noteRepository.GetAllNotes(userId.Value);

        return new GetAllNotesResponse(notes.ToList());

    }
}