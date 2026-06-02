using FluentAssertions;
using HiveLogs.Domain.Environments;
using DeploymentEnvironment = HiveLogs.Domain.Environments.Environment;

namespace HiveLogs.Domain.Tests.Environments;

public class EnvironmentTests
{
    private static readonly DateTimeOffset Now = DateTimeOffset.Parse("2026-06-02T00:00:00Z");
    private static readonly Guid ApplicationId = Guid.NewGuid();

    [Fact]
    public void Create_WithValidName_ShouldSucceed()
    {
        var nameResult = EnvironmentName.Create("production");
        var result = DeploymentEnvironment.Create(ApplicationId, nameResult.Value, Now);

        result.IsSuccess.Should().BeTrue();
        result.Value.Name.Value.Should().Be("production");
    }

    [Fact]
    public void Create_ShouldNormalizeNameToLowercase()
    {
        var nameResult = EnvironmentName.Create("Production");

        nameResult.IsSuccess.Should().BeTrue();
        nameResult.Value.Value.Should().Be("production");

        var entityResult = DeploymentEnvironment.Create(ApplicationId, nameResult.Value, Now);
        entityResult.Value.Name.Value.Should().Be("production");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_WithEmptyName_ShouldFail(string? name)
    {
        var nameResult = EnvironmentName.Create(name);

        nameResult.IsFailure.Should().BeTrue();
        nameResult.Error!.Code.Should().Be("environments.name_required");
    }

    [Theory]
    [InlineData("Prod Env")]
    [InlineData("bad name!")]
    [InlineData("a b")]
    [InlineData("has.dot")]
    public void Create_WithInvalidFormat_ShouldFail(string name)
    {
        var nameResult = EnvironmentName.Create(name);

        nameResult.IsFailure.Should().BeTrue();
        nameResult.Error!.Code.Should().Be("environments.name_invalid");
    }

    [Fact]
    public void Create_WithCustomValidName_ShouldSucceed()
    {
        var nameResult = EnvironmentName.Create("qa-preview_1");

        nameResult.IsSuccess.Should().BeTrue();
        nameResult.Value.Value.Should().Be("qa-preview_1");
    }
}
