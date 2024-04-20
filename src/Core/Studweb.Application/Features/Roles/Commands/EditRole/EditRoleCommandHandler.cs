using ErrorOr;
using Studweb.Application.Abstractions.Messaging;
using Studweb.Application.Contracts.Role;
using Studweb.Application.Persistance;
using Studweb.Domain.Common.Errors;

namespace Studweb.Application.Features.Roles.Commands.EditRole;

public class EditRoleCommandHandler : ICommandHandler<EditRoleCommand, EditRoleResponse>
{
    private readonly IRoleRepository _roleRepository;

    public EditRoleCommandHandler(IRoleRepository roleRepository)
    {
        _roleRepository = roleRepository;
    }
    
    public async Task<ErrorOr<EditRoleResponse>> Handle(EditRoleCommand request, CancellationToken cancellationToken)
    {
        var role = await _roleRepository.GetByIdAsync(request.Id);

        if (role is null)
        {
            return Errors.Role.NotFound;
        }

        await _roleRepository.EditAsync(request.Id, request.Name);

        return new EditRoleResponse("Success");
    }
}