﻿using Studweb.Application.Abstractions.Messaging;
using Studweb.Domain.Aggregates.Users.Entities;

namespace Studweb.Application.Features.Roles.Queries.GetRoles;

public sealed record GetRolesQuery : IQuery<IEnumerable<Role>>;