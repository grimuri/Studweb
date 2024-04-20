using ErrorOr;
using MediatR;
using Studweb.Application.Abstractions.Messaging;
using Studweb.Domain.Entities;

namespace Studweb.Application.Features.Roles.Queries.GetRoleById;

public class GetRoleByIdQueryHandler : IQueryHandler<GetRoleByIdQuery, Role>
{
    public Task<ErrorOr<Role>> Handle(GetRoleByIdQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}