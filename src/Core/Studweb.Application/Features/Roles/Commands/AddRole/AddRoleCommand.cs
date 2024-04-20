using Studweb.Application.Abstractions.Messaging;
using Studweb.Application.Contracts.Role;

namespace Studweb.Application.Features.Roles.Commands.AddRoleCommand;

public record AddRoleCommand(string Name) : ICommand<AddRoleResponse>;