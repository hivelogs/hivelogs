using HiveLogs.Application;
using HiveLogs.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HiveLogs.IoC;

public static class DependencyInjection
{
    public static IServiceCollection AddHiveLogsDependencies(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddApplication();
        services.AddInfrastructure(configuration);
        return services;
    }
}
