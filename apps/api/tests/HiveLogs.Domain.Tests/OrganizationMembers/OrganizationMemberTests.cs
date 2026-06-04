using FluentAssertions;
using HiveLogs.Domain.OrganizationMembers;

namespace HiveLogs.Domain.Tests.OrganizationMembers;

public class OrganizationMemberTests
{
    private static readonly DateTimeOffset Now = DateTimeOffset.Parse("2026-06-04T12:00:00Z");

    [Fact]
    public void Create_WithOwnerRole_ShouldSucceed()
    {
        var organizationId = Guid.NewGuid();
        var userId = Guid.NewGuid();

        var result = OrganizationMember.Create(
            organizationId,
            userId,
            OrganizationMemberRole.Owner,
            Now);

        result.IsSuccess.Should().BeTrue();
        result.Value!.Role.Should().Be(OrganizationMemberRole.Owner);
        result.Value.OrganizationId.Should().Be(organizationId);
        result.Value.UserId.Should().Be(userId);
    }

    [Fact]
    public void Create_WithEmptyOrganizationId_ShouldFail()
    {
        var result = OrganizationMember.Create(
            Guid.Empty,
            Guid.NewGuid(),
            OrganizationMemberRole.Admin,
            Now);

        result.IsFailure.Should().BeTrue();
        result.Error!.Code.Should().Be("organization_members.invalid_ids");
    }
}
