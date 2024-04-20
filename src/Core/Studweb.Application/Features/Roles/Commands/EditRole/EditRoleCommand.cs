using Studweb.Application.Abstractions.Messaging;
using Studweb.Application.Contracts.Role;

namespace Studweb.Application.Features.Roles.Commands.EditRole;

public record EditRoleCommand(
    int Id,
    string Name) : ICommand<EditRoleResponse>;