using Dapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;
using Studweb.Infrastructure.Persistance;
using Testcontainers.MsSql;

namespace Studweb.IntegrationTests.Abstractions;

public class IntegrationTestWebAppFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly MsSqlContainer _dbContainer = new MsSqlBuilder()
        .WithImage("mcr.microsoft.com/mssql/server:2022-latest")
        .WithEnvironment("ACCEPT_EULA", "Y")
        .Build();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var descriptor = services.SingleOrDefault(s =>
                s.ServiceType == typeof(IDbContext));

            if (descriptor is not null)
            {
                services.Remove(descriptor);
            }

            services.AddScoped<IDbContext, DbContext>(serviceProvider => 
                new DbContext(_dbContainer.GetConnectionString()));
        });
    }

    public async Task InitializeAsync() 
    {
        await _dbContainer.StartAsync();
        CreateDb();
    }

    public new async Task DisposeAsync() 
    {
        await _dbContainer.StopAsync();
    }
    
    private void CreateDb()
    {
        string script = File.ReadAllText("../../../../../../src/External/Studweb.Infrastructure/DbScripts/db.sql");
        using (var conn = new SqlConnection(_dbContainer.GetConnectionString()))
        {
            conn.Execute(script);
        }
    }
}