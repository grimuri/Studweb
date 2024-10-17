using Microsoft.AspNetCore.Authorization;
using static Studweb.Api.Common.HttpResultsExtensions;

namespace Studweb.Api.Endpoints;

public static class TestModule
{
    public static void AddTestEnpoints(this IEndpointRouteBuilder app)
    {
        
        app.MapGet("/api/ping", async () =>
        {
            return Ok("ping");
        })
        .RequireAuthorization();
    }
}