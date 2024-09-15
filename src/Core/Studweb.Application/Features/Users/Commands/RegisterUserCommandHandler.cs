using System.Security.Cryptography;
using ErrorOr;
using Studweb.Application.Abstractions.Messaging;
using Studweb.Application.Contracts.Authentication;
using Studweb.Application.Persistance;
using Studweb.Application.Utils;
using Studweb.Domain.Common.Errors;
using Studweb.Domain.Entities;

namespace Studweb.Application.Features.Users.Commands;

public class RegisterUserCommandHandler : ICommandHandler<RegisterUserCommand, RegisterResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public RegisterUserCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ErrorOr<RegisterResponse>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var userId = await _userRepository.GetByEmailAsync(request.Email);
        
        if (userId != 0)
        {
            return Errors.User.DuplicateEmail;
        }

        var user = User.Create(
            request.FirstName,
            request.Lastname,
            request.Email,
            PasswordHasher.HashPassword(request.Password),
            request.Birthday,
            Role.Create("User")
            );
        
        _unitOfWork.BeginTransaction();
        
        await _userRepository.RegisterAsync(user);
        
        _unitOfWork.CommitAndCloseConnection();

        var id = await _userRepository.GetByEmailAsync(user.Email);
        
        
        return new RegisterResponse(id, "Successfully registered! Check your email!");
    }
}