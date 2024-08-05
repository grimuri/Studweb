using System.Security.Cryptography;
using System.Text;

namespace Studweb.Application.Utils;

public static class PasswordHasher
{
    public static string HashPassword(string password)
    {
        var passwordHash = BCrypt.Net.BCrypt.EnhancedHashPassword(password, 13);

        return passwordHash;
    }
}