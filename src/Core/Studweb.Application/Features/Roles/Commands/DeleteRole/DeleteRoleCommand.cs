using Studweb.Application.Common.Messaging;
using Studweb.Application.Contracts.Role;

namespace Studweb.Application.Features.Roles.Commands.DeleteRole;

public record DeleteRoleCommand(int Id) : ICommand<DeleteRoleResponse>;