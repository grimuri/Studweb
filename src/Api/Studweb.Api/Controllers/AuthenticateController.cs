using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Studweb.Application.Contracts.Authentication;
using Studweb.Application.Features.Users.Commands;

namespace Studweb.Api.Controllers;

[Route("api/auth")]
public class AuthenticateController : ApiController
{
    private readonly ISender _sender;

    public AuthenticateController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost]
    public async Task<IActionResult> Register([FromBody] RegisterUserCommand registerUserCommand)
    {
        var response = await _sender.Send(registerUserCommand);

        return response.Match(
            result => Ok(result),
            errors => Problem(errors));
    }
}