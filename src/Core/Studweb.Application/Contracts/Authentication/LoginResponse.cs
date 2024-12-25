namespace Studweb.Application.Contracts.Authentication;

public record LoginResponse(
    int Id,
    string FirstName,
    string LastName,
    string Email,
    DateTime Birthday,
    string Token);