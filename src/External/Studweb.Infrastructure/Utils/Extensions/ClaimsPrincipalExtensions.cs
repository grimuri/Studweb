﻿using System.Security.Claims;

namespace Studweb.Infrastructure.Utils.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static int GetUserId(this ClaimsPrincipal? principal)
    {
        var userId = principal.FindFirstValue(ClaimTypes.NameIdentifier);

        return Convert.ToInt32(userId);
    }
}