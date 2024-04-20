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
        var response = await _sender.Send(new GetRolesQuery());
        return response.Match(
            result => Ok(result),
            errors => Problem(errors.ToString()));
    }

    // [HttpGet("{id}")]
    // public async Task<IActionResult> GetById([FromRoute] int id)
    // {
    //     var response = await _sender.Send(new)
    // }
    
}