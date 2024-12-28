using Studweb.Application.Abstractions.Messaging;
using Studweb.Domain.Aggregates.Users.Entities;

namespace Studweb.Application.Features.Roles.Queries.GetRoleById;

public record GetRoleByIdQuery(int Id) : IQuery<Role>;