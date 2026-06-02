using HiveLogs.Application.Abstractions.Time;

namespace HiveLogs.Application.Tests.TestDoubles;

internal sealed class FakeClock : IClock
{
    public FakeClock(DateTimeOffset utcNow) => UtcNow = utcNow;

    public DateTimeOffset UtcNow { get; set; }
}
