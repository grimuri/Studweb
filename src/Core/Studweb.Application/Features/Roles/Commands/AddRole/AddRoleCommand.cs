using Studweb.Application.Common.Messaging;
using Studweb.Application.Contracts.Role;

namespace Studweb.Application.Features.Roles.Commands.AddRole;

public record AddRoleCommand(string Name) : ICommand<AddRoleResponse>;