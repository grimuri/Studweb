using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Studweb.Application.Persistance;
using Studweb.Infrastructure.Repositories;
using Studweb.Infrastructure.Utils;

namespace Studweb.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, 
        ConfigurationManager configuration)
    {
        services.AddScoped<IRoleRepository, RoleRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddSingleton(serviceProvider =>
        {
            var configuration = serviceProvider.GetRequiredService<IConfiguration>();

            var connectionString = configuration.GetConnectionString("Default") ??
                                   throw new ApplicationException("The connection string is null");

            return new SqlConnectionFactory(connectionString);
        });
        
        return services;
    }
}