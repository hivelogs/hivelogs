using HiveLogs.Application.Abstractions.Persistence;

namespace HiveLogs.Application.Tests.TestDoubles;

internal sealed class FakeUnitOfWork : IUnitOfWork
{
    public int SaveChangesCallCount { get; private set; }

    public Exception? ExceptionToThrowOnSave { get; set; }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        SaveChangesCallCount++;

        if (ExceptionToThrowOnSave is not null)
            throw ExceptionToThrowOnSave;

        return Task.FromResult(1);
    }
}
