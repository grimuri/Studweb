namespace Studweb.Application.Contracts.Authentication;

public record RegisterRequest(
    string Name,
    string Surname,
    string Email,
    string Password,
    string ConfirmPassword
    );