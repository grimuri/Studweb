using Microsoft.AspNetCore.Http;
using Studweb.Application.Utils;
using Studweb.Infrastructure.Utils.Extensions;

namespace Studweb.Infrastructure.Utils;

public sealed class UserContext : IUserContext
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserContext(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public bool? IsAuthenticated =>
        _httpContextAccessor
            .HttpContext?
            .User
            .Identity?
            .IsAuthenticated;

    public int? UserId =>
        _httpContextAccessor
            .HttpContext?
            .User
            .GetUserId();
}