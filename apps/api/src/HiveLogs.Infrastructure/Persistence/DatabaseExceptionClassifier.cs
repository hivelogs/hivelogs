using HiveLogs.Application.Abstractions.Persistence;
using Npgsql;

namespace HiveLogs.Infrastructure.Persistence;

internal sealed class DatabaseExceptionClassifier : IDatabaseExceptionClassifier
{
    public bool IsUniqueConstraintViolation(
        Exception exception,
        string? constraintName = null,
        string? tableName = null)
    {
        for (var current = exception; current is not null; current = current.InnerException)
        {
            if (current is not PostgresException postgresException)
                continue;

            if (postgresException.SqlState != PostgresErrorCodes.UniqueViolation)
                continue;

            if (constraintName is not null
                && !string.Equals(postgresException.ConstraintName, constraintName, StringComparison.Ordinal))
                continue;

            if (tableName is not null
                && !string.Equals(postgresException.TableName, tableName, StringComparison.Ordinal))
                continue;

            return true;
        }

        return false;
    }
}
