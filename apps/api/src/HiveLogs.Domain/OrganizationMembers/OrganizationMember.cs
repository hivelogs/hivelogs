using HiveLogs.Domain.Common.Entities;
using HiveLogs.Domain.Common.Errors;

namespace HiveLogs.Domain.OrganizationMembers;

public sealed class OrganizationMember : Entity
{
    public Guid OrganizationId { get; private set; }

    public Guid UserId { get; private set; }

    public OrganizationMemberRole Role { get; private set; }

    public DateTimeOffset CreatedAt { get; private set; }

    private OrganizationMember()
    {
    }

    public static Result<OrganizationMember> Create(
        Guid organizationId,
        Guid userId,
        OrganizationMemberRole role,
        DateTimeOffset createdAt)
    {
        if (organizationId == Guid.Empty || userId == Guid.Empty)
            return Result<OrganizationMember>.Failure(OrganizationMemberErrors.InvalidIds);

        if (!Enum.IsDefined(typeof(OrganizationMemberRole), role))
            return Result<OrganizationMember>.Failure(OrganizationMemberErrors.InvalidRole);

        return Result<OrganizationMember>.Success(new OrganizationMember
        {
            Id = Guid.NewGuid(),
            OrganizationId = organizationId,
            UserId = userId,
            Role = role,
            CreatedAt = createdAt
        });
    }
}
