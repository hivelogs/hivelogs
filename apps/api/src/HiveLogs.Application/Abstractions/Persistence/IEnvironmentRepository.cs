using DomainEnvironment = HiveLogs.Domain.Environments.Environment;

namespace HiveLogs.Application.Abstractions.Persistence;

public interface IEnvironmentRepository
{
    Task<bool> ExistsByNameInApplicationAsync(
        Guid applicationId,
        string name,
        CancellationToken cancellationToken = default);

    Task<DomainEnvironment?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<bool> BelongsToOrganizationAsync(
        Guid organizationId,
        Guid applicationId,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<DomainEnvironment>> ListByApplicationAsync(
        Guid applicationId,
        CancellationToken cancellationToken = default);

    Task AddAsync(DomainEnvironment environment, CancellationToken cancellationToken = default);
}
