﻿using Studweb.Application.Common.Messaging;
using Studweb.Application.Contracts.Role;
using Studweb.Domain.Aggregates.Users.Entities;

namespace Studweb.Application.Features.Roles.Commands.EditRole;

public record EditRoleCommand(
    int Id,
    string Name) : ICommand<Role>;