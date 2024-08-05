using Dapper;
using ErrorOr;
using Studweb.Application.Contracts.Authentication;
using Studweb.Application.Features.Users.Commands;
using Studweb.Application.Persistance;
using Studweb.Domain.Entities;
using Studweb.Infrastructure.Utils;

namespace Studweb.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly SqlConnectionFactory _sqlConnectionFactory;
    private readonly IRoleRepository _roleRepository;

    public UserRepository(SqlConnectionFactory sqlConnectionFactory, IRoleRepository roleRepository)
    {
        _sqlConnectionFactory = sqlConnectionFactory;
        _roleRepository = roleRepository;
    }

    public async Task<bool> AnyAsync(string email, CancellationToken cancellationToken = default)
    {
        using var connection = _sqlConnectionFactory.Create();

        const string sql = @"SELECT COUNT(Id) FROM Users WHERE Email = @Email";

        var isExist = await connection.ExecuteScalarAsync<int>(sql, new { Email = email });

        return Convert.ToBoolean(isExist);
    }

    public async Task<int> RegisterAsync(User user, CancellationToken cancellationToken = default)
    {
        using var connection = _sqlConnectionFactory.Create();

        var role = await _roleRepository.GetByNameAsync(user.Role.Name);
        var roleId = role.Id;

        const string sql = @"INSERT INTO Users (Name, Surname, Email, Password, Birthday, CreatedAt, VerifiedAt,
                                VerificationToken, VerificationTokenExpires, ResetPasswordToken,
                                ResetPasswordTokenExpires, BanTime, RoleId) 
                            OUTPUT Inserted.Id
                            VALUES (@Name, @Surname, @Email, @Password, @Birthday, @CreatedAt, @VerifiedAt,
                                @VerificationToken, @VerificationTokenExpires, @ResetPasswordToken,
                                @ResetPasswordTokenExpires, @BanTime, @RoleId)";

        var id = await connection.ExecuteScalarAsync<int>(sql, new
        {
            Name = user.Name,
            Surname = user.Surname,
            Email = user.Email,
            Password = user.Password,
            Birthday = user.Birthday,
            CreatedAt = user.CreatedAt,
            VerifiedAt = user.VerifiedAt,
            VerificationToken = user.VerificationToken,
            VerificationTokenExpires = user.VerificationTokenExpires,
            ResetPasswordToken = user.ResetPasswordToken,
            ResetPasswordTokenExpires = user.ResetPasswordTokenExpires,
            BanTime = user.BanTime,
            RoleId = roleId
        });

        return id;
    }
}