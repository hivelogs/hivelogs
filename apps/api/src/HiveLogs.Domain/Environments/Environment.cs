using HiveLogs.Domain.Common.Entities;
using HiveLogs.Domain.Common.Errors;

namespace HiveLogs.Domain.Environments;

public sealed class Environment : AuditableEntity
{
    public Guid ApplicationId { get; private set; }

    public EnvironmentName Name { get; private set; } = null!;

    private Environment()
    {
    }

    public static Result<Environment> Create(
        Guid applicationId,
        EnvironmentName name,
        DateTimeOffset now)
    {
        var environment = new Environment
        {
            Id = Guid.NewGuid(),
            ApplicationId = applicationId,
            Name = name,
            CreatedAt = now,
            UpdatedAt = now
        };

        return Result<Environment>.Success(environment);
    }
}
