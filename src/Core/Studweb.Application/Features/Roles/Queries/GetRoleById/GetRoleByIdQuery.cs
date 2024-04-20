using Studweb.Application.Abstractions.Messaging;
using Studweb.Domain.Entities;

namespace Studweb.Application.Features.Roles.Queries.GetRoleById;

public record GetRoleByIdQuery() : IQuery<Role>;