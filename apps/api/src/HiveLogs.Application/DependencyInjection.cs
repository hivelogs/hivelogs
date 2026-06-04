using FluentValidation;
using HiveLogs.Application.Applications;
using HiveLogs.Application.Environments;
using HiveLogs.Application.Organizations;
using Microsoft.Extensions.DependencyInjection;

namespace HiveLogs.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);

        services.AddScoped<IOrganizationService, OrganizationService>();
        services.AddScoped<IApplicationService, ApplicationService>();
        services.AddScoped<IEnvironmentService, EnvironmentService>();

        return services;
    }
}
