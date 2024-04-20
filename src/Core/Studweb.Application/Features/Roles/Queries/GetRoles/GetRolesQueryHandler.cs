using MediatR;
using Studweb.Application.Abstractions.Messaging;
using Studweb.Application.Common;
using Studweb.Application.Persistance;
using Studweb.Domain.Entities;

namespace Studweb.Application.Features.Roles.Queries.GetRoles;

public class GetRolesQueryHandler : IQueryHandler<GetRolesQuery, IEnumerable<Role>>
{
    private readonly IRoleRepository _roleRepository;

    public GetRolesQueryHandler(IRoleRepository roleRepository)
    {
        _roleRepository = roleRepository;
    }
    
    public async Task<Result<IEnumerable<Role>>> Handle(GetRolesQuery request, CancellationToken cancellationToken)
    {
        var roles = await _roleRepository.GetAllAsync();

        //return Result.Success<IEnumerable<Role>>(roles);
        return Result<IEnumerable<Role>>.Success(roles);
    }
}