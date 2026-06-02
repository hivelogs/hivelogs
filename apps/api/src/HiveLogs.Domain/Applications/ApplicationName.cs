using HiveLogs.Domain.Common.Errors;

namespace HiveLogs.Domain.Applications;

public sealed record ApplicationName
{
    public const int MinLength = 2;
    public const int MaxLength = 100;

    public string Value { get; }

    private ApplicationName(string value) => Value = value;

    public static Result<ApplicationName> Create(string? name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return Result<ApplicationName>.Failure(ApplicationErrors.NameRequired);

        var trimmed = name.Trim();

        if (trimmed.Length < MinLength)
            return Result<ApplicationName>.Failure(ApplicationErrors.NameRequired);

        if (trimmed.Length > MaxLength)
            return Result<ApplicationName>.Failure(ApplicationErrors.NameTooLong);

        return Result<ApplicationName>.Success(new ApplicationName(trimmed));
    }
}
