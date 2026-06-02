using HiveLogs.Application.Abstractions.Persistence;
using HiveLogs.Domain.Applications;

namespace HiveLogs.Application.Tests.TestDoubles;

internal sealed class InMemoryApplicationRepository : IApplicationRepository
{
    private readonly List<MonitoredApplication> _applications = [];

    public Task<bool> ExistsByNameInOrganizationAsync(
        Guid organizationId,
        string name,
        CancellationToken cancellationToken = default) =>
        Task.FromResult(_applications.Any(a =>
            a.OrganizationId == organizationId &&
            string.Equals(a.Name.Value, name, StringComparison.OrdinalIgnoreCase)));

    public Task<MonitoredApplication?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        Task.FromResult(_applications.SingleOrDefault(a => a.Id == id));

    public Task<bool> BelongsToOrganizationAsync(
        Guid organizationId,
        Guid applicationId,
        CancellationToken cancellationToken = default) =>
        Task.FromResult(_applications.Any(a =>
            a.Id == applicationId && a.OrganizationId == organizationId));

    public Task<IReadOnlyList<MonitoredApplication>> ListByOrganizationAsync(
        Guid organizationId,
        CancellationToken cancellationToken = default) =>
        Task.FromResult<IReadOnlyList<MonitoredApplication>>(
            _applications.Where(a => a.OrganizationId == organizationId).ToList());

    public Task AddAsync(MonitoredApplication application, CancellationToken cancellationToken = default)
    {
        _applications.Add(application);
        return Task.CompletedTask;
    }
}
