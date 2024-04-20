using MediatR;
using Microsoft.AspNetCore.Mvc;
using Studweb.Application.Features.Roles.Commands.AddRoleCommand;
using Studweb.Application.Features.Roles.Queries.GetRoleById;
using Studweb.Application.Features.Roles.Queries.GetRoles;
using Studweb.Domain.Entities;

namespace Studweb.Api.Controllers;

[Route("api/role")]
public class RoleController : ApiController
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
            errors => Problem(errors));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        var response = await _sender.Send(new GetRoleByIdQuery(id));

        return response.Match(
            result => Ok(result),
            errors => Problem(errors));
    }

    [HttpPost]
    public async Task<IActionResult> Add([FromBody] string name)
    {
        var response = await _sender.Send(new AddRoleCommand(name));

        return response.Match(
            result => Ok(result),
            errors => Problem(errors));
    }
    
}