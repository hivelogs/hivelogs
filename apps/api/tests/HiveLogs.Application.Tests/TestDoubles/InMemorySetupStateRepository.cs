using HiveLogs.Application.Abstractions.Persistence;
using HiveLogs.Domain.Setup;

namespace HiveLogs.Application.Tests.TestDoubles;

internal sealed class InMemorySetupStateRepository : ISetupStateRepository
{
    private SetupState? _state;

    public Task<SetupState?> GetAsync(CancellationToken cancellationToken = default) =>
        Task.FromResult(_state);

    public Task AddAsync(SetupState setupState, CancellationToken cancellationToken = default)
    {
        if (_state is not null)
            throw new InvalidOperationException(SetupState.ConcurrencyConflictMessage);

        _state = setupState;
        return Task.CompletedTask;
    }

    public Task UpdateAsync(SetupState setupState, CancellationToken cancellationToken = default)
    {
        _state = setupState;
        return Task.CompletedTask;
    }

    public SetupState? State => _state;
}
