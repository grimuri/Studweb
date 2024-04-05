using Microsoft.AspNetCore.Mvc;
using Studweb.Application.Contracts.Authentication;

namespace Studweb.Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthenticateController : ControllerBase
{

    [HttpPost]
    public IActionResult Register([FromBody] RegisterRequest registerRequest)
    {
        //Todo: Register endpoint
        return Ok("Zarejestrowano");
    }
}