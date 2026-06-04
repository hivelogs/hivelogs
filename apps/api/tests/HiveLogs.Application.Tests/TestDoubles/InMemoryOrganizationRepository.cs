using HiveLogs.Application.Abstractions.Persistence;
using HiveLogs.Domain.Organizations;

namespace HiveLogs.Application.Tests.TestDoubles;

internal sealed class InMemoryOrganizationRepository : IOrganizationRepository
{
    private readonly List<Organization> _organizations = [];

    public Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken = default) =>
        Task.FromResult(_organizations.Any(o =>
            string.Equals(o.Name.Value, name, StringComparison.OrdinalIgnoreCase)));

    public Task<Organization?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        Task.FromResult(_organizations.SingleOrDefault(o => o.Id == id));

    public Task<IReadOnlyList<Organization>> ListAsync(CancellationToken cancellationToken = default) =>
        Task.FromResult<IReadOnlyList<Organization>>(_organizations.ToList());

    public Task AddAsync(Organization organization, CancellationToken cancellationToken = default)
    {
        _organizations.Add(organization);
        return Task.CompletedTask;
    }
}
