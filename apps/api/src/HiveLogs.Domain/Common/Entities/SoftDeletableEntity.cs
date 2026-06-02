namespace HiveLogs.Domain.Common.Entities;

public abstract class SoftDeletableEntity : AuditableEntity
{
    public DateTimeOffset? DeletedAt { get; protected set; }

    public bool IsDeleted => DeletedAt is not null;
}
