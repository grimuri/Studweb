using System.Security.Cryptography;
using Dapper;
using Studweb.Domain.Aggregates.User.Entities;
using Studweb.Domain.Common.Interfaces;
using Studweb.Infrastructure.Persistance;
using Studweb.Infrastructure.Utils.TempClasses;
using Studweb.IntegrationTests.TestUtils.DataToSeed;

namespace Studweb.IntegrationTests.TestUtils;

public static class DataSeeder
{
    private const int SaltSize = 16;
    private const int HashSize = 32;
    private const int Iterations = 10000;

    private static readonly HashAlgorithmName Algorithm = HashAlgorithmName.SHA512;
    
    public static void Seed(IDbContext dbContext)
    {
        AddRole(dbContext);
        AddUser(dbContext);
        dbContext.Connection.Close();
    }

    private static void AddRole(IDbContext dbContext)
    {
        using var connection = dbContext.Create();

        var role = RoleToSeed.GivenRoleUser();
        
        // Check if role exists
        const string isExistRoleSql = @"SELECT * FROM Roles WHERE Name = @Name";
        var isExist = connection.QueryFirstOrDefault<RoleTemp>(isExistRoleSql, new { Name = role.Name });

        if (isExist.Id != null)
        {
            return;
        }
        
        // Add role to db
        const string addRoleSql = @"INSERT INTO Roles (Name) OUTPUT Inserted.Id VALUES (@Name)";
        connection.ExecuteScalar(addRoleSql, new { Name = role.Name });
    }
    
    private static void AddUser(IDbContext dbContext)
    {
        using var connection = dbContext.Connection;

        var user = UserToSeed.GivenUser();
        
        // Get role
        const string getRoleSql = @"SELECT Id, Name FROM Roles WHERE Name = @Name";
        var roleTemp = connection.QueryFirstOrDefault<RoleTemp>(getRoleSql, new { Name = user.Role.Name });
        
        // Check if role id is not null
        if (roleTemp.Id == null)
        {
            throw new Exception("Role id is null");
        }
        
        // Get role id
        var roleId = roleTemp.Id;
        
        // Check if user exists
        const string isExistUserSql = @"SELECT * FROM Users WHERE Email = @Email";
        var userTemp = connection.QueryFirstOrDefault<dynamic>(isExistUserSql, new { Email = user.Email });
        if (userTemp != null)
        {
            return;
        }
        
        // Add tokens to db

        var verificationTokenId = 0;
        if (user.VerificationToken?.Value is not null)
        {
            verificationTokenId = AddToken(user.VerificationToken, dbContext);
        }

        var resetPasswordTokenId = 0;
        if (user.ResetPasswordToken?.Value is not null)
        {
            resetPasswordTokenId = AddToken(user.ResetPasswordToken, dbContext);
        }
        
        // Add user to db
        const string addUserSql = @"INSERT INTO Users (
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
            Password = Hash(user.Password),
            Birthday = user.Birthday,
            CreatedOnUtc = DateTime.UtcNow,
            VerifiedOnUtc = DateTime.UtcNow,
            LastModifiedPasswordOnUtc = DateTime.UtcNow,
            BanTime = (object)DBNull.Value,
            VerificationTokenId = verificationTokenId != 0 ? verificationTokenId : (object)DBNull.Value,
            ResetPasswordTokenId = resetPasswordTokenId != 0 ? resetPasswordTokenId : (object)DBNull.Value,
            RoleId = roleId
        };

        connection.ExecuteScalar(addUserSql, parameters);
    }

    private static int AddToken(IToken token, IDbContext dbContext)
    {
        var connection = dbContext.Connection;
        
        const string addTokenSql = @"INSERT INTO Tokens (Value, CreatedOnUtc, ExpiresOnUtc, Type) 
                            OUTPUT Inserted.Id
                            VALUES (@Value, @CreatedOnUtc, @ExpiresOnUtc, @Type)";

        var parameters = new
        {
            Value = token.Value,
            CreatedOnUtc = token.CreatedOnUtc,
            ExpiresOnUtc = token.ExpiresOnUtc,
            Type = token.Type,
        };
        
        var id = connection.ExecuteScalar<int>(addTokenSql, token);

        return id;
    }
    
    
    private static string Hash(string password)
    {
        byte[] salt = RandomNumberGenerator.GetBytes(SaltSize);
        byte[] hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, Iterations, Algorithm, HashSize);

        return $"{Convert.ToHexString(hash)}-{Convert.ToHexString(salt)}";
    }
}