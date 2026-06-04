using HiveLogs.Domain.OrganizationMembers;

namespace HiveLogs.Application.Abstractions.Persistence;

public interface IOrganizationMemberRepository
{
    Task AddAsync(OrganizationMember member, CancellationToken cancellationToken = default);
}
