using System.Text.RegularExpressions;
using HiveLogs.Domain.Common.Errors;

namespace HiveLogs.Domain.Environments;

public sealed partial record EnvironmentName
{
    public const int MinLength = 1;
    public const int MaxLength = 64;

    public string Value { get; }

    private EnvironmentName(string value) => Value = value;

    public static Result<EnvironmentName> Create(string? name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return Result<EnvironmentName>.Failure(EnvironmentErrors.NameRequired);

        var normalized = name.Trim().ToLowerInvariant();

        if (normalized.Length < MinLength)
            return Result<EnvironmentName>.Failure(EnvironmentErrors.NameRequired);

        if (normalized.Length > MaxLength)
            return Result<EnvironmentName>.Failure(EnvironmentErrors.NameTooLong);

        if (!NamePattern().IsMatch(normalized))
            return Result<EnvironmentName>.Failure(EnvironmentErrors.NameInvalid);

        return Result<EnvironmentName>.Success(new EnvironmentName(normalized));
    }

    [GeneratedRegex("^[a-z0-9_-]+$")]
    private static partial Regex NamePattern();
}
