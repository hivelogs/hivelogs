using HiveLogs.Application.Abstractions.Persistence;
using DomainEnvironment = HiveLogs.Domain.Environments.Environment;
using HiveLogs.Domain.Environments;
using Microsoft.EntityFrameworkCore;

namespace HiveLogs.Infrastructure.Persistence.Repositories;

internal sealed class EnvironmentRepository(HiveLogsDbContext dbContext) : IEnvironmentRepository
{
    public Task<bool> ExistsByNameInApplicationAsync(
        Guid applicationId,
        string name,
        CancellationToken cancellationToken = default)
    {
        var nameResult = EnvironmentName.Create(name);
        if (nameResult.IsFailure)
            return Task.FromResult(false);

        var environmentName = nameResult.Value;
        return dbContext.Environments.AnyAsync(
            e => e.ApplicationId == applicationId && e.Name == environmentName,
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
            .OrderBy(e => e.Name)
            .ToListAsync(cancellationToken);

    public Task AddAsync(DomainEnvironment environment, CancellationToken cancellationToken = default)
    {
        dbContext.Environments.Add(environment);
        return Task.CompletedTask;
    }
}
