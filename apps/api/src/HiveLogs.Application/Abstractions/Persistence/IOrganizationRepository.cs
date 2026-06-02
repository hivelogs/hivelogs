using HiveLogs.Domain.Organizations;

namespace HiveLogs.Application.Abstractions.Persistence;

public interface IOrganizationRepository
{
    Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken = default);

    Task<Organization?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Organization>> ListAsync(CancellationToken cancellationToken = default);

    Task AddAsync(Organization organization, CancellationToken cancellationToken = default);
}
