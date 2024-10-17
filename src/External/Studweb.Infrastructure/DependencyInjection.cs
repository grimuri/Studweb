using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Quartz;
using Studweb.Application.Persistance;
using Studweb.Application.Utils;
using Studweb.Infrastructure.Authentication;
using Studweb.Infrastructure.BackgroundJobs;
using Studweb.Infrastructure.Outbox;
using Studweb.Infrastructure.Persistance;
using Studweb.Infrastructure.Repositories;
using Studweb.Infrastructure.Utilities;
using Studweb.Infrastructure.Utils;

namespace Studweb.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, 
        ConfigurationManager configuration)
    {
        services.AddScoped<IRoleRepository, RoleRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ITokenRepository, TokenRepository>();
        services.AddScoped<IDbContext, DbContext>(serviceProvider =>
        {
            var config = serviceProvider.GetRequiredService<IConfiguration>();
            var connectionString = config.GetConnectionString("Default") ??
                                   throw new ApplicationException("The connection string is null");
            
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ApplicationException("The connection string is null or empty");
            }
            
            return new DbContext(connectionString);
        });
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IOutboxMessageRepository, OutboxMessageRepository>();
        services.AddQuartz(config =>
        {
            var jobKey = new JobKey(nameof(ProcessOutboxMessagesJob));

            config
                .AddJob<ProcessOutboxMessagesJob>(jobKey)
                .AddTrigger(
                    trigger =>
                        trigger.ForJob(jobKey)
                            .WithSimpleSchedule(
                                schedule =>
                                    schedule.WithIntervalInSeconds(10)
                                        .RepeatForever()));
        });
        services.AddQuartzHostedService();
        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.AddScoped<IJwtProvider, JwtProvider>();
        
        return services;
    }
}