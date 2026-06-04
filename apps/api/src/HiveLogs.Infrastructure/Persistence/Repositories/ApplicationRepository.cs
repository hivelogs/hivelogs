using HiveLogs.Application.Abstractions.Persistence;
using HiveLogs.Domain.Applications;
using Microsoft.EntityFrameworkCore;

namespace HiveLogs.Infrastructure.Persistence.Repositories;

internal sealed class ApplicationRepository(HiveLogsDbContext dbContext) : IApplicationRepository
{
    public Task<bool> ExistsByNameInOrganizationAsync(
        Guid organizationId,
        string name,
        CancellationToken cancellationToken = default)
    {
        var normalized = name.Trim().ToLowerInvariant();
        return dbContext.Applications.AnyAsync(
            a => a.OrganizationId == organizationId &&
                 EF.Property<string>(a, "NameLower") == normalized,
            cancellationToken);
    }

    public Task<MonitoredApplication?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        dbContext.Applications.SingleOrDefaultAsync(a => a.Id == id, cancellationToken);

    public Task<bool> BelongsToOrganizationAsync(
        Guid organizationId,
        Guid applicationId,
        CancellationToken cancellationToken = default) =>
        dbContext.Applications.AnyAsync(
            a => a.Id == applicationId && a.OrganizationId == organizationId,
            cancellationToken);

    public async Task<IReadOnlyList<MonitoredApplication>> ListByOrganizationAsync(
        Guid organizationId,
        CancellationToken cancellationToken = default) =>
        await dbContext.Applications
            .Where(a => a.OrganizationId == organizationId)
            .OrderBy(a => EF.Property<string>(a, "NameLower"))
            .ToListAsync(cancellationToken);

    public Task AddAsync(MonitoredApplication application, CancellationToken cancellationToken = default)
    {
        dbContext.Applications.Add(application);
        return Task.CompletedTask;
    }
}
