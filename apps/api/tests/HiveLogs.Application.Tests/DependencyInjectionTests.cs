using FluentAssertions;
using HiveLogs.Application;
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
}
