using HiveLogs.Domain.Common.Errors;

namespace HiveLogs.Domain.Organizations;

public sealed record OrganizationName
{
    public const int MinLength = 2;
    public const int MaxLength = 100;

    public string Value { get; }

    private OrganizationName(string value) => Value = value;

    public static Result<OrganizationName> Create(string? name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return Result<OrganizationName>.Failure(OrganizationErrors.NameRequired);

        var trimmed = name.Trim();

        if (trimmed.Length < MinLength)
            return Result<OrganizationName>.Failure(OrganizationErrors.NameRequired);

        if (trimmed.Length > MaxLength)
            return Result<OrganizationName>.Failure(OrganizationErrors.NameTooLong);

        return Result<OrganizationName>.Success(new OrganizationName(trimmed));
    }
}
