using Dapper;
using Microsoft.IdentityModel.Tokens;
using Studweb.Application.Persistance;
using Studweb.Domain.Aggregates.User;
using Studweb.Domain.Aggregates.User.Entities;
using Studweb.Domain.Aggregates.User.Enums;
using Studweb.Domain.Aggregates.User.ValueObjects;
using Studweb.Infrastructure.Outbox;
using Studweb.Infrastructure.Persistance;
using Studweb.Infrastructure.Utilities;
using Studweb.Infrastructure.Utils;
using Studweb.Infrastructure.Utils.TempClasses;

namespace Studweb.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly IDbContext _dbContext;
    private readonly IOutboxMessageRepository _outboxMessageRepository;
    private readonly IRoleRepository _roleRepository;

    public UserRepository(IDbContext dbContext, IRoleRepository roleRepository,
        IOutboxMessageRepository outboxMessageRepository)
    {
        _dbContext = dbContext;
        _roleRepository = roleRepository;
        _outboxMessageRepository = outboxMessageRepository;
    }

    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        var connection = _dbContext.Connection;

        // TODO: Fix this
        const string sql = @"
                            SELECT 
                                u.Id,
                                u.FirstName,
                                u.LastName,
                                u.Email,
                                u.Password,
                                u.Birthday,
                                u.CreatedOnUtc,
                                u.VerifiedOnUtc,
                                u.LastModifiedPasswordOnUtc,
                                u.BanTime,
                                r.Id AS RoleId,
                                r.Name AS RoleName,
                                vt.Id AS VerificationTokenId,
                                vt.Value AS VerificationTokenValue,
                                vt.CreatedOnUtc AS VerificationTokenCreatedOnUtc,
                                vt.ExpiresOnUtc AS VerificationTokenExpiresOnUtc,
                                vt.Type AS VerificationTokenType,
                                rt.Id AS ResetPasswordTokenId,
                                rt.Value AS ResetPasswordTokenValue,
                                rt.CreatedOnUtc AS ResetPasswordTokenCreatedOnUtc,
                                rt.ExpiresOnUtc AS ResetPasswordTokenExpiresOnUtc,
                                rt.Type AS ResetPasswordTokenType
                            FROM Users u
                            LEFT JOIN Tokens vt ON vt.Id = u.VerificationTokenId
                            LEFT JOIN Tokens rt ON rt.Id = u.ResetPasswordTokenId
                            LEFT JOIN Roles r ON r.Id = u.RoleId
                            WHERE u.Email = @Email;";

    // Pobieranie danych użytkownika i tokenów
    var userWithTokens = await connection.QueryFirstOrDefaultAsync<UserWithTokensTemp>(sql, new { Email = email });

    if (userWithTokens is null)
    {
        return null;
    }
    
    return userWithTokens.ParseToUser();
}

    public async Task RegisterAsync(User user, CancellationToken cancellationToken = default)
    {
        var connection = _dbContext.Connection;
        var transaction = _dbContext.Transaction;

        var role = await _roleRepository.GetByNameAsync(user.Role.Name)
                   ?? throw new ApplicationException("Role with that name doesn't exist");
        var roleId = role?.Id.Value;
        
        const string sql = @"INSERT INTO Users (
                               FirstName, 
                               LastName, 
                               Email, 
                               Password, 
                               Birthday, 
                               CreatedOnUtc, 
                               VerifiedOnUtc,
                               LastModifiedPasswordOnUtc,
                               BanTime,
                               VerificationTokenId,
                               ResetPasswordTokenId,
                               RoleId
                               ) 
                            OUTPUT Inserted.Id
                            VALUES (
                               @FirstName, 
                               @LastName, 
                               @Email, 
                               @Password, 
                               @Birthday, 
                               @CreatedOnUtc, 
                               @VerifiedOnUtc,
                               @LastModifiedPasswordOnUtc,
                               @BanTime,
                               @VerificationTokenId,
                               @ResetPasswordTokenId,
                               @RoleId)";

        var parameters = new
        {
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            Password = user.Password,
            Birthday = user.Birthday,
            CreatedOnUtc = user.CreatedOnUtc,
            VerifiedOnUtc = user.VerifiedOnUtc,
            LastModifiedPasswordOnUtc = user.LastModifiedPasswordOnUtc,
            BanTime = user.BanTime,
            VerificationTokenId = (object)DBNull.Value,
            ResetPasswordTokenId = (object)DBNull.Value,
            RoleId = roleId
        };

        await connection.ExecuteScalarAsync(sql, parameters, transaction);

        if (!user.DomainEvents.IsNullOrEmpty())
        {
            var domainEvents = user.DomainEvents.ToList();
            user.ClearDomainEvents();
            foreach (var domainEvent in domainEvents) await _outboxMessageRepository.SaveDomainEvents(domainEvent);
        }
    }

    public async Task EditVerificationTokenAsync(int userId, int verificationTokenId, CancellationToken cancellationToken = default)
    {
        var connection = _dbContext.Connection;

        const string sql = @"UPDATE Users 
                            SET VerificationTokenId = @VerificationTokenId
                            WHERE Id = @UserId";

        var parameters = new
        {
            VerificationTokenId = verificationTokenId,
            UserId = userId,
        };

        await connection.ExecuteScalarAsync(sql, parameters);

    }
}