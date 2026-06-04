using FluentAssertions;
using HiveLogs.Domain.Organizations;

namespace HiveLogs.Domain.Tests.Organizations;

public class OrganizationTests
{
    private static readonly DateTimeOffset Now = DateTimeOffset.Parse("2026-06-02T00:00:00Z");

    [Fact]
    public void Create_WithValidName_ShouldSucceed()
    {
        var nameResult = OrganizationName.Create("My Company");
        var result = Organization.Create(nameResult.Value, Now);

        result.IsSuccess.Should().BeTrue();
        result.Value.Name.Value.Should().Be("My Company");
        result.Value.CreatedAt.Should().Be(Now);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("A")]
    public void Create_WithInvalidName_ShouldFail(string? name)
    {
        var nameResult = OrganizationName.Create(name);

        nameResult.IsFailure.Should().BeTrue();
        nameResult.Error!.Code.Should().Be("organizations.name_required");
    }

    [Fact]
    public void Create_WithNameTooLong_ShouldFail()
    {
        var nameResult = OrganizationName.Create(new string('a', OrganizationName.MaxLength + 1));

        nameResult.IsFailure.Should().BeTrue();
        nameResult.Error!.Code.Should().Be("organizations.name_too_long");
    }
}
