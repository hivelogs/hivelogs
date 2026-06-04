using HiveLogs.Application.Abstractions.Persistence;
using HiveLogs.Domain.OrganizationMembers;

namespace HiveLogs.Infrastructure.Persistence.Repositories;

internal sealed class OrganizationMemberRepository : IOrganizationMemberRepository
{
    private readonly HiveLogsDbContext _dbContext;

    public OrganizationMemberRepository(HiveLogsDbContext dbContext) => _dbContext = dbContext;

    public async Task AddAsync(OrganizationMember member, CancellationToken cancellationToken = default) =>
        await _dbContext.OrganizationMembers.AddAsync(member, cancellationToken);
}
