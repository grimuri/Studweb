using ErrorOr;
using Studweb.Application.Common.Messaging;
using Studweb.Application.Contracts.Role;
using Studweb.Application.Persistance;
using Studweb.Domain.Common.Errors;

namespace Studweb.Application.Features.Roles.Commands.DeleteRole;

public class DeleteRoleCommandHandler : ICommandHandler<DeleteRoleCommand, DeleteRoleResponse>
{
    private readonly IRoleRepository _roleRepository;

    public DeleteRoleCommandHandler(IRoleRepository roleRepository)
    {
        _roleRepository = roleRepository;
    }

    public async Task<ErrorOr<DeleteRoleResponse>> Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
    {
        var role = await _roleRepository.GetByIdAsync(request.Id);

        if (role is null)
        {
            return Errors.Role.NotFound;
        }

        var affectedRows = await _roleRepository.DeleteAsync(request.Id);

        return new DeleteRoleResponse(affectedRows);
    }
}