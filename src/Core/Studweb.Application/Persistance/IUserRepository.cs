using Studweb.Application.Contracts.Authentication;
using Studweb.Application.Features.Users.Commands;
using Studweb.Domain.Entities;

namespace Studweb.Application.Persistance;

public interface IUserRepository
{
    Task<int> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task RegisterAsync(User request, CancellationToken cancellationToken = default);
}