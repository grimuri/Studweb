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

    public RegisterUserCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<ErrorOr<RegisterResponse>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var isExist = await _userRepository.AnyAsync(request.Email);

        if (isExist)
        {
            return Errors.User.DuplicateEmail;
        }

        var user = User.Create(
            request.FirstName,
            request.Lastname,
            request.Email,
            PasswordHasher.HashPassword(request.Password),
            request.Birthday,
            new Role()
            {
                Name = "User",
            }
            );
        // var users = new User()
        // {
        //     FirstName =  request.FirstName,
        //     LastName = request.Lastname,
        //     Birthday = request.Birthday,
        //     Email = request.Email,
        //     Password = PasswordHasher.HashPassword(request.Password),
        //     VerificationToken = RandomNumberGenerator.GetBytes(64).ToString(),
        //     VerificationTokenExpires = DateTime.Now.AddDays(3),
        //     Role = new Role()
        //     {
        //         Name = "User"
        //     }
        // };

        var id = await _userRepository.RegisterAsync(user);

        return new RegisterResponse(id, "Successfully registered! Check your email!");
    }
}