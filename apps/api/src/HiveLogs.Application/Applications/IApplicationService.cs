using HiveLogs.Application.Applications.Requests;
using HiveLogs.Application.Applications.Responses;
using HiveLogs.Domain.Common.Errors;

namespace HiveLogs.Application.Applications;

public interface IApplicationService
{
    Task<Result<ApplicationResponse>> CreateAsync(
        Guid organizationId,
        CreateApplicationRequest request,
        CancellationToken cancellationToken = default);

    Task<Result<ApplicationResponse>> GetByIdAsync(
        Guid organizationId,
        Guid applicationId,
        CancellationToken cancellationToken = default);

    Task<Result<IReadOnlyList<ApplicationResponse>>> ListByOrganizationAsync(
        Guid organizationId,
        CancellationToken cancellationToken = default);
}
