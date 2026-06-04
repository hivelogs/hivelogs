using System.Security.Cryptography;
using System.Text;
using HiveLogs.Application.Abstractions.Security;
using HiveLogs.Domain.Common.Errors;
using HiveLogs.Domain.Setup;
using Microsoft.Extensions.Configuration;

namespace HiveLogs.Infrastructure.Security;

internal sealed class SetupPasswordValidator : ISetupPasswordValidator
{
    private readonly string? _configuredPassword;

    public SetupPasswordValidator(IConfiguration configuration)
    {
        _configuredPassword = configuration["HIVELOGS_SETUP_PASSWORD"]
            ?? configuration["Setup:Password"];
    }

    public Result Validate(string? setupPassword)
    {
        if (string.IsNullOrWhiteSpace(_configuredPassword))
            return Result.Failure(SetupErrors.PasswordNotConfigured);

        if (string.IsNullOrWhiteSpace(setupPassword))
            return Result.Failure(SetupErrors.InvalidSetupPassword);

        if (!SecureEquals(_configuredPassword, setupPassword))
            return Result.Failure(SetupErrors.InvalidSetupPassword);

        return Result.Success();
    }

    private static bool SecureEquals(string expected, string provided)
    {
        var expectedBytes = Encoding.UTF8.GetBytes(expected);
        var providedBytes = Encoding.UTF8.GetBytes(provided);

        if (expectedBytes.Length != providedBytes.Length)
            return false;

        return CryptographicOperations.FixedTimeEquals(expectedBytes, providedBytes);
    }
}
