using HiveLogs.Application.Abstractions.Persistence;
using DomainEnvironment = HiveLogs.Domain.Environments.Environment;
using Microsoft.EntityFrameworkCore;

namespace HiveLogs.Infrastructure.Persistence.Repositories;

internal sealed class EnvironmentRepository(HiveLogsDbContext dbContext) : IEnvironmentRepository
{
    public Task<bool> ExistsByNameInApplicationAsync(
        Guid applicationId,
        string name,
        CancellationToken cancellationToken = default)
    {
        var normalized = name.Trim().ToLowerInvariant();
        return dbContext.Environments.AnyAsync(
            e => e.ApplicationId == applicationId && e.Name.Value == normalized,
            cancellationToken);
    }

    public Task<DomainEnvironment?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        dbContext.Environments.SingleOrDefaultAsync(e => e.Id == id, cancellationToken);

    public Task<bool> BelongsToOrganizationAsync(
        Guid organizationId,
        Guid applicationId,
        CancellationToken cancellationToken = default) =>
        dbContext.Applications.AnyAsync(
            a => a.Id == applicationId && a.OrganizationId == organizationId,
            cancellationToken);

    public async Task<IReadOnlyList<DomainEnvironment>> ListByApplicationAsync(
        Guid applicationId,
        CancellationToken cancellationToken = default) =>
        await dbContext.Environments
            .Where(e => e.ApplicationId == applicationId)
            .OrderBy(e => e.Name.Value)
            .ToListAsync(cancellationToken);

    public Task AddAsync(DomainEnvironment environment, CancellationToken cancellationToken = default)
    {
        dbContext.Environments.Add(environment);
        return Task.CompletedTask;
    }
}
