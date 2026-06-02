using HiveLogs.Application.Abstractions.Time;

namespace HiveLogs.Infrastructure.Time;

public sealed class SystemClock : IClock
{
    public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;
}
