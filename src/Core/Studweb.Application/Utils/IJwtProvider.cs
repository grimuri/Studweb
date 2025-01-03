using Studweb.Domain.Aggregates.Users;

namespace Studweb.Application.Utils;

public interface IJwtProvider
{
    string Generate(User user);
}