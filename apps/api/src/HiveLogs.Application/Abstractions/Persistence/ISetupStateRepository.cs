using HiveLogs.Domain.Setup;

namespace HiveLogs.Application.Abstractions.Persistence;

public interface ISetupStateRepository
{
    Task<SetupState?> GetAsync(CancellationToken cancellationToken = default);

    Task AddAsync(SetupState setupState, CancellationToken cancellationToken = default);

    Task UpdateAsync(SetupState setupState, CancellationToken cancellationToken = default);
}
