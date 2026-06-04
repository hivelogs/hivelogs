using FluentAssertions;
using HiveLogs.Application;
using HiveLogs.Application.Applications;
using HiveLogs.Application.Environments;
using HiveLogs.Application.Organizations;
using Microsoft.Extensions.DependencyInjection;

namespace HiveLogs.Application.Tests;

public class DependencyInjectionTests
{
    [Fact]
    public void AddApplication_ShouldRegisterServicesWithoutThrowing()
    {
        var services = new ServiceCollection();

        var act = () => services.AddApplication();

        act.Should().NotThrow();

        var provider = services.BuildServiceProvider();
        provider.Should().NotBeNull();
    }

    [Fact]
    public void AddApplication_ShouldRegisterCoreServicesAsScoped()
    {
        var services = new ServiceCollection();
        services.AddApplication();

        services.Should().Contain(d =>
            d.ServiceType == typeof(IOrganizationService) &&
            d.ImplementationType == typeof(OrganizationService) &&
            d.Lifetime == ServiceLifetime.Scoped);
        services.Should().Contain(d =>
            d.ServiceType == typeof(IApplicationService) &&
            d.ImplementationType == typeof(ApplicationService) &&
            d.Lifetime == ServiceLifetime.Scoped);
        services.Should().Contain(d =>
            d.ServiceType == typeof(IEnvironmentService) &&
            d.ImplementationType == typeof(EnvironmentService) &&
            d.Lifetime == ServiceLifetime.Scoped);
    }
}
