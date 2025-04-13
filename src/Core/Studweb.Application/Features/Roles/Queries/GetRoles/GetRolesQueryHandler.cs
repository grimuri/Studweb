using ErrorOr;
using MediatR;
using Studweb.Application.Common.Messaging;
using Studweb.Application.Persistance;
using Studweb.Domain.Aggregates.Users.Entities;

namespace Studweb.Application.Features.Roles.Queries.GetRoles;

public class GetRolesQueryHandler : IQueryHandler<GetRolesQuery, IEnumerable<Role>>
{
    private readonly IRoleRepository _roleRepository;

    public GetRolesQueryHandler(IRoleRepository roleRepository)
    {
        _roleRepository = roleRepository;
    }

    public async Task<ErrorOr<IEnumerable<Role>>> Handle(GetRolesQuery request, CancellationToken cancellationToken)
    {
        var roles = await _roleRepository.GetAllAsync();

        return roles.ToList();
    }
}