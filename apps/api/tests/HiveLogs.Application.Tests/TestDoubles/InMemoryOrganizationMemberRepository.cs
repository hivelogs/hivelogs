using HiveLogs.Application.Abstractions.Persistence;
using HiveLogs.Domain.OrganizationMembers;

namespace HiveLogs.Application.Tests.TestDoubles;

internal sealed class InMemoryOrganizationMemberRepository : IOrganizationMemberRepository
{
    private readonly List<OrganizationMember> _members = [];

    public Task AddAsync(OrganizationMember member, CancellationToken cancellationToken = default)
    {
        _members.Add(member);
        return Task.CompletedTask;
    }

    public IReadOnlyList<OrganizationMember> Members => _members;
}
