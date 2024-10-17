using Studweb.Domain.Aggregates.User;

namespace Studweb.Application.Utils;

public interface IJwtProvider
{
    string Generate(User user);
}