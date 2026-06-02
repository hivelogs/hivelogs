using FluentAssertions;
using HiveLogs.Domain.Applications;

namespace HiveLogs.Domain.Tests.Applications;

public class MonitoredApplicationTests
{
    private static readonly DateTimeOffset Now = DateTimeOffset.Parse("2026-06-02T00:00:00Z");
    private static readonly Guid OrganizationId = Guid.NewGuid();

    [Fact]
    public void Create_WithValidName_ShouldSucceed()
    {
        var nameResult = ApplicationName.Create("Checkout API");
        var result = MonitoredApplication.Create(OrganizationId, nameResult.Value, Now);

        result.IsSuccess.Should().BeTrue();
        result.Value.OrganizationId.Should().Be(OrganizationId);
        result.Value.Name.Value.Should().Be("Checkout API");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("X")]
    public void Create_WithInvalidName_ShouldFail(string? name)
    {
        var nameResult = ApplicationName.Create(name);

        nameResult.IsFailure.Should().BeTrue();
        nameResult.Error!.Code.Should().Be("applications.name_required");
    }
}
