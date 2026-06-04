using HiveLogs.Domain.Applications;
using HiveLogs.Domain.Organizations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace HiveLogs.Infrastructure.Persistence;

internal sealed class NameLowerSynchronizationInterceptor : SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(
        DbContextEventData eventData,
        InterceptionResult<int> result)
    {
        SynchronizeNameLower(eventData.Context);
        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        SynchronizeNameLower(eventData.Context);
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private static void SynchronizeNameLower(DbContext? context)
    {
        if (context is null || !context.Database.IsInMemory())
            return;

        foreach (var entry in context.ChangeTracker.Entries<Organization>())
        {
            if (entry.State is not (EntityState.Added or EntityState.Modified))
                continue;

            entry.Property("NameLower").CurrentValue = entry.Entity.Name.Value.ToLowerInvariant();
        }

        foreach (var entry in context.ChangeTracker.Entries<MonitoredApplication>())
        {
            if (entry.State is not (EntityState.Added or EntityState.Modified))
                continue;

            entry.Property("NameLower").CurrentValue = entry.Entity.Name.Value.ToLowerInvariant();
        }
    }
}
