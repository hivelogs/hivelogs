using HiveLogs.Application.Abstractions.Persistence;

namespace HiveLogs.Application.Tests.TestDoubles;

internal sealed class FakeDatabaseExceptionClassifier : IDatabaseExceptionClassifier
{
    public bool SetupStateUniqueViolation { get; set; }

    public bool IsUniqueConstraintViolation(
        Exception exception,
        string? constraintName = null,
        string? tableName = null)
    {
        if (tableName == "setup_state" && SetupStateUniqueViolation)
            return true;

        return false;
    }
}
