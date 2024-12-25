using ErrorOr;
using MediatR;
using Studweb.Application.Abstractions.Messaging;
using Studweb.Application.Persistance;
using Studweb.Domain.Aggregates.User.Entities;
using Studweb.Domain.Common.Errors;

namespace Studweb.Application.Features.Roles.Queries.GetRoleById;

public class GetRoleByIdQueryHandler : IQueryHandler<GetRoleByIdQuery, Role>
{
    private readonly IRoleRepository _roleRepository;

    public GetRoleByIdQueryHandler(IRoleRepository roleRepository)
    {
        _roleRepository = roleRepository;
    }
    public async Task<ErrorOr<Role>> Handle(GetRoleByIdQuery request, CancellationToken cancellationToken)
    {
        var role = await _roleRepository.GetByIdAsync(request.Id);

        if (role is null)
        {
            return Errors.Role.NotFound;
        }

        return role;
    }
}