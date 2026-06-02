using HiveLogs.Domain.Common.Entities;
using HiveLogs.Domain.Common.Errors;

namespace HiveLogs.Domain.Organizations;

public sealed class Organization : AuditableEntity
{
    public OrganizationName Name { get; private set; } = null!;

    private Organization()
    {
    }

    public static Result<Organization> Create(OrganizationName name, DateTimeOffset now)
    {
        var organization = new Organization
        {
            Id = Guid.NewGuid(),
            Name = name,
            CreatedAt = now,
            UpdatedAt = now
        };

        return Result<Organization>.Success(organization);
    }
}
