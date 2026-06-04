using HiveLogs.Domain.Applications;

namespace HiveLogs.Application.Abstractions.Persistence;

public interface IApplicationRepository
{
    Task<bool> ExistsByNameInOrganizationAsync(
        Guid organizationId,
        string name,
        CancellationToken cancellationToken = default);

    Task<MonitoredApplication?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<bool> BelongsToOrganizationAsync(
        Guid organizationId,
        Guid applicationId,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<MonitoredApplication>> ListByOrganizationAsync(
        Guid organizationId,
        CancellationToken cancellationToken = default);

    Task AddAsync(MonitoredApplication application, CancellationToken cancellationToken = default);
}
