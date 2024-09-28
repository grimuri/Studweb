using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Studweb.Application.Persistance;
using Studweb.Infrastructure.BackgroundJobs;
using Studweb.Infrastructure.Outbox;
using Studweb.Infrastructure.Persistance;
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
        services.AddScoped<ITokenRepository, TokenRepository>();
        services.AddSingleton(serviceProvider =>
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
        
        return services;
    }
}