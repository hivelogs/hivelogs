using HiveLogs.Application.Setup.Requests;
using HiveLogs.Application.Setup.Responses;
using HiveLogs.Domain.Common.Errors;

namespace HiveLogs.Application.Setup;

public interface ISetupService
{
    Task<Result<SetupStatusResponse>> GetStatusAsync(CancellationToken cancellationToken = default);

    Task<Result<InitializeSetupResponse>> InitializeAsync(
        InitializeSetupRequest request,
        CancellationToken cancellationToken = default);
}
