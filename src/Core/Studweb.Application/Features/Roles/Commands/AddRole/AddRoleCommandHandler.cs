using ErrorOr;
using Studweb.Application.Abstractions.Messaging;
using Studweb.Application.Contracts.Role;
using Studweb.Application.Persistance;
using Studweb.Domain.Aggregates.Users.Entities;
using Studweb.Domain.Common.Errors;

namespace Studweb.Application.Features.Roles.Commands.AddRoleCommand;

public class AddRoleCommandHandler : ICommandHandler<AddRoleCommand, AddRoleResponse>
{
    private readonly IRoleRepository _roleRepository;

    public AddRoleCommandHandler(IRoleRepository roleRepository)
    {
        _roleRepository = roleRepository;
    }
    
    public async Task<ErrorOr<AddRoleResponse>> Handle(AddRoleCommand request, CancellationToken cancellationToken)
    {
        var role = await _roleRepository.GetByNameAsync(request.Name);

        if (role is not null)
        {
            return Errors.Role.DuplicateName;
        }

        var newRole = Role.Create(request.Name);

        var response = await _roleRepository.AddAsync(newRole);

        return new AddRoleResponse(response);
    }
}