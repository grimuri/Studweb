using Studweb.Domain.Common.Interfaces;
using Studweb.Domain.Entities;

namespace Studweb.Application.Persistance;

public interface ITokenRepository
{
    Task<int> AddTokenAsync(IToken token, CancellationToken cancellationToken = default);
    Task<int> GetByIdAsync(int id, CancellationToken cancellationToken = default);
}