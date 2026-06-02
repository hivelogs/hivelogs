using HiveLogs.Application.Environments.Requests;
using HiveLogs.Application.Environments.Responses;
using HiveLogs.Domain.Common.Errors;

namespace HiveLogs.Application.Environments;

public interface IEnvironmentService
{
    Task<Result<EnvironmentResponse>> CreateAsync(
        Guid organizationId,
        Guid applicationId,
        CreateEnvironmentRequest request,
        CancellationToken cancellationToken = default);

    Task<Result<EnvironmentResponse>> GetByIdAsync(
        Guid organizationId,
        Guid applicationId,
        Guid environmentId,
        CancellationToken cancellationToken = default);

    Task<Result<IReadOnlyList<EnvironmentResponse>>> ListByApplicationAsync(
        Guid organizationId,
        Guid applicationId,
        CancellationToken cancellationToken = default);
}
