using System.Security.Claims;
using Dapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;
using Studweb.Infrastructure.Persistance;
using Studweb.IntegrationTests.TestUtils;

namespace Studweb.IntegrationTests.Abstractions;

public abstract class BaseIntegrationTest : IDisposable
{
    private readonly IServiceScope _scope;

    public BaseIntegrationTest(IntegrationTestWebAppFactory factory)
    {
        _scope = factory.Services.CreateScope();
        Sender = _scope.ServiceProvider.GetRequiredService<ISender>();
        DbContext = _scope.ServiceProvider.GetRequiredService<IDbContext>();
        HttpContextAccessor = _scope.ServiceProvider.GetRequiredService<IHttpContextAccessor>();
        HttpContextAccessor.HttpContext = SetAuthenticatedUser();
        DataSeeder.Seed(DbContext);
    }

    protected ISender Sender { get; }
    protected IDbContext DbContext { get; }
    protected IHttpContextAccessor HttpContextAccessor { get; }
    
    public void Dispose()
    {
        _scope.Dispose();
    }
    
    private HttpContext SetAuthenticatedUser()
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, "1"),
            new Claim(ClaimTypes.Email, "email@gmail.com")
        };

        var identity = new ClaimsIdentity(claims, "Test");
        var principal = new ClaimsPrincipal(identity);

        var httpContext = new DefaultHttpContext
        {
            User = principal
        };

        return httpContext;
    }
}