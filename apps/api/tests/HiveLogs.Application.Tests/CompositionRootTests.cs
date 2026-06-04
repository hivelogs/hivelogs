using FluentAssertions;
using HiveLogs.Application.Abstractions.Persistence;
using HiveLogs.Application.Abstractions.Security;
using HiveLogs.Application.Applications;
using HiveLogs.Application.Environments;
using HiveLogs.Application.Organizations;
using HiveLogs.Application.Setup;
using HiveLogs.IoC;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HiveLogs.Application.Tests;

public class CompositionRootTests
{
    [Fact]
    public void AddHiveLogsDependencies_ShouldRegisterCoreServicesAsScoped()
    {
        var services = new ServiceCollection();
        services.AddHiveLogsDependencies(CreateInMemoryTestConfiguration());

        services.Should().Contain(d =>
            d.ServiceType == typeof(IOrganizationService) &&
            d.Lifetime == ServiceLifetime.Scoped);
        services.Should().Contain(d =>
            d.ServiceType == typeof(IApplicationService) &&
            d.Lifetime == ServiceLifetime.Scoped);
        services.Should().Contain(d =>
            d.ServiceType == typeof(IEnvironmentService) &&
            d.Lifetime == ServiceLifetime.Scoped);
        services.Should().Contain(d =>
            d.ServiceType == typeof(ISetupService) &&
            d.Lifetime == ServiceLifetime.Scoped);
    }

    [Fact]
    public void AddHiveLogsDependencies_ShouldRegisterCoreRepositoriesAsScoped()
    {
        var services = new ServiceCollection();
        services.AddHiveLogsDependencies(CreateInMemoryTestConfiguration());

        services.Should().Contain(d =>
            d.ServiceType == typeof(IOrganizationRepository) &&
            d.Lifetime == ServiceLifetime.Scoped);
        services.Should().Contain(d =>
            d.ServiceType == typeof(IApplicationRepository) &&
            d.Lifetime == ServiceLifetime.Scoped);
        services.Should().Contain(d =>
            d.ServiceType == typeof(IEnvironmentRepository) &&
            d.Lifetime == ServiceLifetime.Scoped);
        services.Should().Contain(d =>
            d.ServiceType == typeof(IUserRepository) &&
            d.Lifetime == ServiceLifetime.Scoped);
        services.Should().Contain(d =>
            d.ServiceType == typeof(ISetupStateRepository) &&
            d.Lifetime == ServiceLifetime.Scoped);
        services.Should().Contain(d =>
            d.ServiceType == typeof(IPasswordHasher) &&
            d.Lifetime == ServiceLifetime.Singleton);
    }

    [Fact]
    public void AddHiveLogsDependencies_ShouldResolveServicesAndRepositories()
    {
        var configuration = CreateInMemoryTestConfiguration();
        var services = new ServiceCollection();
        services.AddSingleton<IConfiguration>(configuration);
        services.AddHiveLogsDependencies(configuration);

        using var provider = services.BuildServiceProvider();
        using var scope = provider.CreateScope();
        var sp = scope.ServiceProvider;

        sp.GetRequiredService<IOrganizationService>().Should().NotBeNull();
        sp.GetRequiredService<IApplicationService>().Should().NotBeNull();
        sp.GetRequiredService<IEnvironmentService>().Should().NotBeNull();
        sp.GetRequiredService<IOrganizationRepository>().Should().NotBeNull();
        sp.GetRequiredService<IApplicationRepository>().Should().NotBeNull();
        sp.GetRequiredService<IEnvironmentRepository>().Should().NotBeNull();
        sp.GetRequiredService<ISetupService>().Should().NotBeNull();
    }

    private static IConfiguration CreateInMemoryTestConfiguration() =>
        new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Testing:UseInMemoryDatabase"] = "true",
                ["Testing:InMemoryDatabaseName"] = Guid.NewGuid().ToString(),
                ["Setup:Password"] = "test-setup-password"
            })
            .Build();
}
