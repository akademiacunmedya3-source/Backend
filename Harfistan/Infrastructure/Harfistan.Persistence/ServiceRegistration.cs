using Harfistan.Persistence.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Harfistan.Persistence;

public static class ServiceRegistration
{
    public static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<HarfistanDbContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("HarfistanDB"), npgsqlOptions =>
            {
                npgsqlOptions.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);

                npgsqlOptions.CommandTimeout(30);

                npgsqlOptions.MigrationsAssembly(typeof(HarfistanDbContext).Assembly.FullName);
            });

            if (configuration["ASPNETCORE_ENVIRONMENT"] == "Development")
            {
                options.EnableSensitiveDataLogging();
                options.EnableDetailedErrors();
            }
        } );
        
        services.AddHealthChecks().AddDbContextCheck<HarfistanDbContext>();
        
        return services;
    }
}