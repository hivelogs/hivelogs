using HiveLogs.Domain.Common.Entities;

namespace HiveLogs.Domain.Setup;

public sealed class SetupState : Entity
{
    public bool IsCompleted { get; private set; }

    public DateTimeOffset? CompletedAt { get; private set; }

    private SetupState()
    {
    }

    public static SetupState CreatePending()
    {
        return new SetupState
        {
            Id = Guid.NewGuid(),
            IsCompleted = false,
            CompletedAt = null
        };
    }

    public void MarkCompleted(DateTimeOffset completedAt)
    {
        IsCompleted = true;
        CompletedAt = completedAt;
    }

    public SetupStatus GetStatus() =>
        IsCompleted ? SetupStatus.Configured : SetupStatus.SetupRequired;
}
