using Studweb.Application.Abstractions.Messaging;
using Studweb.Application.Contracts.Role;
using Studweb.Domain.Aggregates.User.Entities;

namespace Studweb.Application.Features.Roles.Commands.EditRole;

public record EditRoleCommand(
    int Id,
    string Name) : ICommand<Role>;