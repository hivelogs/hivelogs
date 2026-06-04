using HiveLogs.Domain.Common.Errors;

namespace HiveLogs.Domain.Users;

public sealed record UserName
{
    public const int MinLength = 1;
    public const int MaxLength = 100;

    public string Value { get; }

    private UserName(string value) => Value = value;

    public static Result<UserName> Create(string? name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return Result<UserName>.Failure(UserErrors.NameRequired);

        var trimmed = name.Trim();

        if (trimmed.Length < MinLength)
            return Result<UserName>.Failure(UserErrors.NameRequired);

        if (trimmed.Length > MaxLength)
            return Result<UserName>.Failure(UserErrors.NameTooLong);

        return Result<UserName>.Success(new UserName(trimmed));
    }
}
