using Dapper;
using ErrorOr;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Studweb.Application.Contracts.Authentication;
using Studweb.Application.Features.Users.Commands;
using Studweb.Application.Persistance;
using Studweb.Domain.Entities;
using Studweb.Domain.Primitives;
using Studweb.Infrastructure.Outbox;
using Studweb.Infrastructure.Utils;

namespace Studweb.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly DbContext _dbContext;
    private readonly IRoleRepository _roleRepository;
    private readonly IOutboxMessageRepository _outboxMessageRepository;

    public UserRepository(DbContext dbContext, IRoleRepository roleRepository, IOutboxMessageRepository outboxMessageRepository)
    {
        _dbContext = dbContext;
        _roleRepository = roleRepository;
        _outboxMessageRepository = outboxMessageRepository;
    }

    public async Task<int> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        var connection = _dbContext.Connection;

        const string sql = @"SELECT Id FROM Users WHERE Email = @Email";

        var id = await connection.ExecuteScalarAsync<int>(sql, new { Email = email });

        return id;
    }

    public async Task RegisterAsync(User user, CancellationToken cancellationToken = default)
    {
        var connection = _dbContext.Connection;
        var transaction = _dbContext.Transaction;
        
        
        var role = await _roleRepository.GetByNameAsync(user.Role.Name) 
                   ?? throw new ApplicationException("Role with that name doesn't exist");
        var roleId = role?.Id.Value;

        const string sql = @"INSERT INTO Users (FirstName, LastName, Email, Password, Birthday, CreatedAt, VerifiedAt,
                                VerificationToken, VerificationTokenExpires, ResetPasswordToken,
                                ResetPasswordTokenExpires, BanTime, RoleId) 
                            OUTPUT Inserted.Id
                            VALUES (@FirstName, @LastName, @Email, @Password, @Birthday, @CreatedAt, @VerifiedAt,
                                @VerificationToken, @VerificationTokenExpires, @ResetPasswordToken,
                                @ResetPasswordTokenExpires, @BanTime, @RoleId)";

        var parameters = new
        {
            FirstName = user.FirstName,
            LastName = user.LastName,
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
        };
        
        await connection.ExecuteScalarAsync(sql, parameters, transaction);

        if (!user.DomainEvents.IsNullOrEmpty())
        {
            var domainEvents = user.DomainEvents.ToList();
            user.ClearDomainEvents();
            foreach (var domainEvent in domainEvents)
            {
                await _outboxMessageRepository.SaveDomainEvents(domainEvent);
            }
        }
        
    }
}