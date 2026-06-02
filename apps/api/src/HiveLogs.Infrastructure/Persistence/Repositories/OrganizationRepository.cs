using HiveLogs.Application.Abstractions.Persistence;
using HiveLogs.Domain.Organizations;
using Microsoft.EntityFrameworkCore;

namespace HiveLogs.Infrastructure.Persistence.Repositories;

internal sealed class OrganizationRepository(HiveLogsDbContext dbContext) : IOrganizationRepository
{
    public Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        var normalized = name.Trim().ToLowerInvariant();
        return dbContext.Organizations.AnyAsync(
            o => o.Name.Value.ToLower() == normalized,
            cancellationToken);
    }

    public Task<Organization?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        dbContext.Organizations.SingleOrDefaultAsync(o => o.Id == id, cancellationToken);

    public async Task<IReadOnlyList<Organization>> ListAsync(CancellationToken cancellationToken = default) =>
        await dbContext.Organizations
            .OrderBy(o => o.Name.Value)
            .ToListAsync(cancellationToken);

    public Task AddAsync(Organization organization, CancellationToken cancellationToken = default)
    {
        dbContext.Organizations.Add(organization);
        return Task.CompletedTask;
    }
}
