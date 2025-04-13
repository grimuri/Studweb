namespace Studweb.Application.Contracts.Authentication;

public record VerifyEmailRequest(
    string Email,
    string VerificationToken
    );