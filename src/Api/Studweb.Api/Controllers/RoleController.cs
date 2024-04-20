using MediatR;
using Microsoft.AspNetCore.Mvc;
using Studweb.Application.Features.Roles.Queries.GetRoles;
using Studweb.Domain.Entities;

namespace Studweb.Api.Controllers;

[ApiController]
[Route("api/role")]
public class RoleController : ControllerBase
{
    private readonly ISender _sender;

    public RoleController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var roles = await _sender.Send(new GetRolesQuery());
        return Ok(roles.Response);
    }
    
}