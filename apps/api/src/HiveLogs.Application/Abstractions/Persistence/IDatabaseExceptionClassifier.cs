namespace HiveLogs.Application.Abstractions.Persistence;

public interface IDatabaseExceptionClassifier
{
    bool IsUniqueConstraintViolation(
        Exception exception,
        string? constraintName = null,
        string? tableName = null);
}
