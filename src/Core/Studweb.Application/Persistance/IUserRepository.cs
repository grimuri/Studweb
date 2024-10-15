using Studweb.Application.Contracts.Authentication;
using Studweb.Application.Features.Users.Commands;
using Studweb.Domain.Aggregates.User;

namespace Studweb.Application.Persistance;

public interface IUserRepository
{
    Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task RegisterAsync(User request, CancellationToken cancellationToken = default);
    Task EditVerificationTokenAsync(int userId, int verificationTokenId, CancellationToken cancellationToken = default);
}