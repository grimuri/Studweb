using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Studweb.Api.Common;
using Studweb.Application.Features.Users.Commands;
using Studweb.Application.Features.Users.Commands.LoginUser;
using Studweb.Application.Features.Users.Commands.RegisterUser;
using static Studweb.Api.Common.HttpResultsExtensions;

namespace Studweb.Api.Endpoints;

public static class AuthenticationModule
{
    public static void AddAuthenticationEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/register", async (
            [FromBody] RegisterUserCommand command,
            [FromServices] ISender sender) =>
        {
            var response = await sender.Send(command);

            return response.Match(
                result => Ok(result),
                errors => Problem(errors));
        });

        app.MapPost("/api/login", async (
            [FromBody] LoginUserCommand command,
            [FromServices] ISender sender) =>
        {
            var response = await sender.Send(command);

            return response.Match(
                result => Ok(result),
                errors => Problem(errors));
        });

    }
}