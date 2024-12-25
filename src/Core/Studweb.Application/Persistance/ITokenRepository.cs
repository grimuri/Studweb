using Studweb.Domain.Common.Interfaces;

namespace Studweb.Application.Persistance;

public interface ITokenRepository
{
    Task<int> AddTokenAsync(IToken token, CancellationToken cancellationToken = default);
    Task<int> GetByIdAsync(int id, CancellationToken cancellationToken = default);
}