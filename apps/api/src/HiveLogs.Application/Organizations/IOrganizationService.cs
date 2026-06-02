using HiveLogs.Application.Organizations.Requests;
using HiveLogs.Application.Organizations.Responses;
using HiveLogs.Domain.Common.Errors;

namespace HiveLogs.Application.Organizations;

public interface IOrganizationService
{
    Task<Result<OrganizationResponse>> CreateAsync(
        CreateOrganizationRequest request,
        CancellationToken cancellationToken = default);

    Task<Result<OrganizationResponse>> GetByIdAsync(
        Guid organizationId,
        CancellationToken cancellationToken = default);

    Task<Result<IReadOnlyList<OrganizationResponse>>> ListAsync(
        CancellationToken cancellationToken = default);
}
