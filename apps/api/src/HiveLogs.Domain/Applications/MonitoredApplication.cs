using HiveLogs.Domain.Common.Entities;
using HiveLogs.Domain.Common.Errors;

namespace HiveLogs.Domain.Applications;

public sealed class MonitoredApplication : AuditableEntity
{
    public Guid OrganizationId { get; private set; }

    public ApplicationName Name { get; private set; } = null!;

    private MonitoredApplication()
    {
    }

    public static Result<MonitoredApplication> Create(
        Guid organizationId,
        ApplicationName name,
        DateTimeOffset now)
    {
        var application = new MonitoredApplication
        {
            Id = Guid.NewGuid(),
            OrganizationId = organizationId,
            Name = name,
            CreatedAt = now,
            UpdatedAt = now
        };

        return Result<MonitoredApplication>.Success(application);
    }
}
