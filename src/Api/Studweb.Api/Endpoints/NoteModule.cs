using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Studweb.Application.Contracts.Notes.Requests;
using Studweb.Application.Features.Notes.Commands.AddNote;
using Studweb.Application.Features.Notes.Commands.DeleteNote;
using Studweb.Application.Features.Notes.Commands.UpdateNote;
using Studweb.Application.Features.Notes.Queries.GetAllNotes;
using Studweb.Application.Features.Notes.Queries.GetNote;
using static Studweb.Api.Common.HttpResultsExtensions;

namespace Studweb.Api.Endpoints;

public static class NoteModule
{
    public static void AddNoteEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/note", async (
                [FromBody] AddNoteCommand command,
                [FromServices] ISender sender
            ) =>
            {
                var response = await sender.Send(command);

                return response.Match(
                    result => Ok(result),
                    errors => Problem(errors));
            })
            .RequireAuthorization();

        app.MapGet("/api/note", async (
                [FromServices] ISender sender
            ) =>
            {
                var response = await sender.Send(new GetAllNotesQuery());

                return response.Match(
                    result => Ok(result),
                    errors => Problem(errors));
            })
            .RequireAuthorization();

        app.MapGet("/api/note/{id}", async (
                [FromRoute] int id,
                [FromServices] ISender sender
            ) =>
            {
                var response = await sender.Send(new GetNoteQuery(id));

                return response.Match(
                    result => Ok(result),
                    errors => Problem(errors));
            })
            .RequireAuthorization();

        app.MapPut("/api/note/{id}", async (
                [FromRoute] int id,
                [FromBody] UpdateNoteRequest updateNoteRequest,
                [FromServices] ISender sender
            ) =>
            {
                var updateNoteCommand = new UpdateNoteCommand(
                    id,
                    updateNoteRequest.Title,
                    updateNoteRequest.Content,
                    updateNoteRequest.Tags);

                var response = await sender.Send(updateNoteCommand);

                return response.Match(
                    result => Ok(result),
                    errors => Problem(errors));
            })
            .RequireAuthorization();

        app.MapDelete("/api/note/{id}", async (
                [FromRoute] int id,
                [FromServices] ISender sender
            ) =>
            {
                var deleteNoteCommand = new DeleteNoteCommand(id);

                var response = await sender.Send(deleteNoteCommand);

                return response.Match(
                    result => Ok(result),
                    errors => Problem(errors));
            })
            .RequireAuthorization();
    }
}