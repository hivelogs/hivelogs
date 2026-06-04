using HiveLogs.Application.Abstractions.Persistence;
using DomainEnvironment = HiveLogs.Domain.Environments.Environment;

namespace HiveLogs.Application.Tests.TestDoubles;

internal sealed class InMemoryEnvironmentRepository : IEnvironmentRepository
{
    private readonly List<DomainEnvironment> _environments = [];
    private readonly InMemoryApplicationRepository _applicationRepository;

    public InMemoryEnvironmentRepository(InMemoryApplicationRepository applicationRepository) =>
        _applicationRepository = applicationRepository;

    public Task<bool> ExistsByNameInApplicationAsync(
        Guid applicationId,
        string name,
        CancellationToken cancellationToken = default) =>
        Task.FromResult(_environments.Any(e =>
            e.ApplicationId == applicationId &&
            string.Equals(e.Name.Value, name, StringComparison.Ordinal)));

    public Task<DomainEnvironment?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        Task.FromResult(_environments.SingleOrDefault(e => e.Id == id));

    public Task<bool> BelongsToOrganizationAsync(
        Guid organizationId,
        Guid applicationId,
        CancellationToken cancellationToken = default) =>
        _applicationRepository.BelongsToOrganizationAsync(organizationId, applicationId, cancellationToken);

    public Task<IReadOnlyList<DomainEnvironment>> ListByApplicationAsync(
        Guid applicationId,
        CancellationToken cancellationToken = default) =>
        Task.FromResult<IReadOnlyList<DomainEnvironment>>(
            _environments.Where(e => e.ApplicationId == applicationId).ToList());

    public Task AddAsync(DomainEnvironment environment, CancellationToken cancellationToken = default)
    {
        _environments.Add(environment);
        return Task.CompletedTask;
    }
}
