using ErrorOr;
using Studweb.Application.Common.Messaging;
using Studweb.Application.Contracts.Authentication;
using Studweb.Application.Persistance;
using Studweb.Application.Utils;
using Studweb.Domain.Aggregates.Users;
using Studweb.Domain.Aggregates.Users.Entities;
using Studweb.Domain.Aggregates.Users.ValueObjects;
using Studweb.Domain.Common.Errors;

namespace Studweb.Application.Features.Users.Commands.RegisterUser;

public class RegisterUserCommandHandler : ICommandHandler<RegisterUserCommand, RegisterResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordHasher _passwordHasher;

    public RegisterUserCommandHandler(
        IUserRepository userRepository, 
        IUnitOfWork unitOfWork, 
        IPasswordHasher passwordHasher)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _passwordHasher = passwordHasher;
    }

    public async Task<ErrorOr<RegisterResponse>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var userId = await _userRepository.GetByEmailAsync(request.Email);
        
        if (userId is not null)
        {
            return Errors.User.DuplicateEmail;
        }

        var user = User.Create(
            UserId.Create(-1), 
            request.FirstName,
            request.LastName,
            request.Email,
            _passwordHasher.Hash(request.Password),
            request.Birthday,
            Role.Create("User")
            );
        
        _unitOfWork.BeginTransaction();
        
        await _userRepository.RegisterAsync(user);
        
        _unitOfWork.CommitAndCloseConnection();

        var newUser = await _userRepository.GetByEmailAsync(user.Email);
        var id = newUser.Id.Value;
        
        
        return new RegisterResponse(id, "Successfully registered! Check your email!");
    }
}