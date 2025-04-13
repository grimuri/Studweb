using ErrorOr;
using Studweb.Application.Common.Messaging;
using Studweb.Application.Contracts.Notes;
using Studweb.Application.Contracts.Notes.Dtos;
using Studweb.Application.Contracts.Notes.Responses;
using Studweb.Application.Persistance;
using Studweb.Application.Utils;
using Studweb.Domain.Aggregates.Notes;
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
        
        var notes = await _noteRepository.GetAllNotesAsync(userId.Value);

        var notesDto = notes
            .Select(x => new NoteDto(
                x.Id.Value,
                x.Title,
                x.Content,
                x.CreatedOnUtc,
                x.LastModifiedOnUtc));
        
        return new GetAllNotesResponse(notesDto.ToList());

    }
}