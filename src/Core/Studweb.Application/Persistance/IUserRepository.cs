using Studweb.Application.Contracts.Authentication;
using Studweb.Application.Features.Users.Commands;
using Studweb.Domain.Entities;
using Studweb.Domain.Entities.ValueObjects;

namespace Studweb.Application.Persistance;

public interface IUserRepository
{
    Task<int> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task RegisterAsync(User request, CancellationToken cancellationToken = default);
    Task EditVerificationTokenAsync(int userId, int verificationTokenId, CancellationToken cancellationToken = default);
}