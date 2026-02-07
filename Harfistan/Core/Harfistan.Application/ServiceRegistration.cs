using System.Reflection;
using FluentValidation;
using Harfistan.Application.BackgroundServices;
using Microsoft.Extensions.DependencyInjection;

namespace Harfistan.Application;

public static class ServiceRegistration
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddMediator(options => options.ServiceLifetime = ServiceLifetime.Scoped);
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddHostedService<DailyWordCreatorService>();
        return services;
    }
}  