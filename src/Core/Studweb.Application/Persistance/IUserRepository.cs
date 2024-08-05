using Studweb.Application.Contracts.Authentication;
using Studweb.Application.Features.Users.Commands;
using Studweb.Domain.Entities;

namespace Studweb.Application.Persistance;

public interface IUserRepository
{
    Task<bool> AnyAsync(string email, CancellationToken cancellationToken = default);
    Task<int> RegisterAsync(User request, CancellationToken cancellationToken = default);
}