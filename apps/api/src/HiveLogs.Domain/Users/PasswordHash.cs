using HiveLogs.Domain.Common.Errors;

namespace HiveLogs.Domain.Users;

public sealed record PasswordHash
{
    public string Value { get; }

    private PasswordHash(string value) => Value = value;

    public static Result<PasswordHash> Create(string? hash)
    {
        if (string.IsNullOrWhiteSpace(hash))
            return Result<PasswordHash>.Failure(UserErrors.PasswordHashRequired);

        return Result<PasswordHash>.Success(new PasswordHash(hash));
    }
}
