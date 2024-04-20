using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Studweb.Application.Persistance;
using Studweb.Infrastructure.Repositories;

namespace Studweb.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, 
        ConfigurationManager configuration)
    {
        services.AddScoped<IRoleRepository, RoleRepository>();
        
        return services;
    }
}