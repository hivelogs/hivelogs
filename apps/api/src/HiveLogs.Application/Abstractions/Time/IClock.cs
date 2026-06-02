namespace HiveLogs.Application.Abstractions.Time;

public interface IClock
{
    DateTimeOffset UtcNow { get; }
}
