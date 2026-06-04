using HiveLogs.Application.Abstractions.Persistence;
using HiveLogs.Domain.Setup;
using Microsoft.EntityFrameworkCore;

namespace HiveLogs.Infrastructure.Persistence.Repositories;

internal sealed class SetupStateRepository : ISetupStateRepository
{
    private readonly HiveLogsDbContext _dbContext;

    public SetupStateRepository(HiveLogsDbContext dbContext) => _dbContext = dbContext;

    public Task<SetupState?> GetAsync(CancellationToken cancellationToken = default) =>
        _dbContext.SetupStates.OrderBy(s => s.Id).FirstOrDefaultAsync(cancellationToken);

    public async Task AddAsync(SetupState setupState, CancellationToken cancellationToken = default) =>
        await _dbContext.SetupStates.AddAsync(setupState, cancellationToken);

    public Task UpdateAsync(SetupState setupState, CancellationToken cancellationToken = default)
    {
        _dbContext.SetupStates.Update(setupState);
        return Task.CompletedTask;
    }
}
