using Studweb.Domain.Aggregates.User;
using Studweb.Domain.Aggregates.User.Entities;
using Studweb.Domain.Aggregates.User.Enums;
using Studweb.Domain.Aggregates.User.ValueObjects;

namespace Studweb.Infrastructure.Utils.TempClasses;

public class UserWithTokensTemp
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public DateTime Birthday { get; set; }
    public DateTime CreatedOnUtc { get; set; }
    public DateTime? VerifiedOnUtc { get; set; }
    public DateTime? LastModifiedPasswordOnUtc { get; set; }
    public DateTime? BanTime { get; set; } 
    public int RoleId { get; set; }
    public string RoleName { get; set; } = string.Empty;

    // VerificationToken
    public int? VerificationTokenId { get; set; }
    public Guid? VerificationTokenValue { get; set; }
    public DateTime? VerificationTokenCreatedOnUtc { get; set; } 
    public DateTime? VerificationTokenExpiresOnUtc { get; set; }
    public int? VerificationTokenType { get; set; } 

    // ResetPasswordToken
    public int? ResetPasswordTokenId { get; set; }
    public Guid? ResetPasswordTokenValue { get; set; } 
    public DateTime? ResetPasswordTokenCreatedOnUtc { get; set; } 
    public DateTime? ResetPasswordTokenExpiresOnUtc { get; set; } 
    public int? ResetPasswordTokenType { get; set; }

    public User ParseToUser()
    {
        var user = User
            .Empty()
            .Load(
                id: UserId.Create(Id),
                firstName: FirstName,
                lastName: LastName,
                email: Email,
                password: Password,
                birthday: Birthday,
                createdOnUtc: CreatedOnUtc,
                verifiedOnUtc: VerifiedOnUtc,
                lastModifiedPasswordOnUtc: LastModifiedPasswordOnUtc,
                banTime: BanTime,
                role: Role.Create(RoleName, Domain.Aggregates.User.ValueObjects.RoleId.Create(RoleId)),
                verificationToken: LoadVerificationToken(),
                resetPasswordToken: LoadResetPasswordToken()
            );
        
        return user;
    }

    private VerificationToken? LoadVerificationToken()
    {
        if (VerificationTokenId is null) return null;
        
        var verificationToken = VerificationToken
            .Create()
            .Load(
                id: Domain.Aggregates.User.ValueObjects.VerificationTokenId.Create((int)VerificationTokenId),
                value: (Guid)VerificationTokenValue,
                createdOnUtc: (DateTime)VerificationTokenCreatedOnUtc,
                expiresOnUtc: (DateTime)VerificationTokenExpiresOnUtc,
                tokenType: (TokenType)VerificationTokenType
            );

        return verificationToken;
    }
    
    private ResetPasswordToken? LoadResetPasswordToken()
    {
        if (ResetPasswordTokenId is null) return null;
        
        var resetPasswordToken = ResetPasswordToken
            .Create()
            .Load(
                id: Domain.Aggregates.User.ValueObjects.ResetPasswordTokenId.Create((int)ResetPasswordTokenId),
                value: (Guid)ResetPasswordTokenValue,
                createdOnUtc: (DateTime)ResetPasswordTokenCreatedOnUtc,
                expiresOnUtc: (DateTime)ResetPasswordTokenExpiresOnUtc,
                tokenType: (TokenType)ResetPasswordTokenType
            );

        return resetPasswordToken;
    }
    
}