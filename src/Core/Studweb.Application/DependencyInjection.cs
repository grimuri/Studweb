using System.Net;
using System.Net.Mail;
using System.Reflection;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Studweb.Application.Common.Behavior;
using Microsoft.Extensions.Configuration;

namespace Studweb.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(
        this IServiceCollection services,
        ConfigurationManager configuration)
    {
        services.AddMediatR(configuration =>
            configuration.RegisterServicesFromAssemblies(typeof(DependencyInjection).Assembly));

        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        services
            .AddFluentEmail(
                configuration["Email:SenderEmail"],
                configuration["Email:Sender"])
            .AddSmtpSender(
                new SmtpClient(configuration["Email:Host"])
                {
                    Port = Convert.ToInt32(configuration["Email:Port"]),
                    Credentials = new NetworkCredential(
                        configuration["Email:Username"],
                        configuration["Email:Password"]),
                    EnableSsl = true,
                });
        throw new Exception(configuration["Email:Host"]);
        return services;
    }
}