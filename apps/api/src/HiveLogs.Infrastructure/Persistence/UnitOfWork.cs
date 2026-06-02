using HiveLogs.Application.Abstractions.Persistence;

namespace HiveLogs.Infrastructure.Persistence;

public sealed class UnitOfWork(HiveLogsDbContext dbContext) : IUnitOfWork
{
    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) =>
        dbContext.SaveChangesAsync(cancellationToken);
}
