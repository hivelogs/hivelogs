using HiveLogs.Application.Abstractions.Security;
using HiveLogs.Domain.Common.Errors;
using HiveLogs.Domain.Setup;

namespace HiveLogs.Application.Tests.TestDoubles;

internal sealed class FakeSetupPasswordValidator : ISetupPasswordValidator
{
    private readonly string? _expectedPassword;

    public FakeSetupPasswordValidator(string? expectedPassword = "setup-secret")
    {
        _expectedPassword = expectedPassword;
    }

    public Result Validate(string? setupPassword)
    {
        if (string.IsNullOrWhiteSpace(_expectedPassword))
            return Result.Failure(SetupErrors.PasswordNotConfigured);

        if (setupPassword != _expectedPassword)
            return Result.Failure(SetupErrors.InvalidSetupPassword);

        return Result.Success();
    }
}
