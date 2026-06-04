using HiveLogs.Domain.Common.Entities;

namespace HiveLogs.Domain.Setup;

public sealed class SetupState : Entity
{
    public static readonly Guid SingletonId =
        Guid.Parse("00000000-0000-0000-0000-000000000001");

    public const string ConcurrencyConflictMessage = "SETUP_STATE_ALREADY_EXISTS";

    public bool IsCompleted { get; private set; }

    public DateTimeOffset? CompletedAt { get; private set; }

    private SetupState()
    {
    }

    public static SetupState CreatePending()
    {
        return new SetupState
        {
            Id = SingletonId,
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
