using System.Net.Mail;
using HiveLogs.Domain.Common.Errors;

namespace HiveLogs.Domain.Users;

public sealed record Email
{
    public const int MaxLength = 320;

    public string Value { get; }

    private Email(string value) => Value = value;

    public static Result<Email> Create(string? email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return Result<Email>.Failure(UserErrors.EmailRequired);

        var trimmed = email.Trim();

        if (trimmed.Length > MaxLength)
            return Result<Email>.Failure(UserErrors.EmailTooLong);

        try
        {
            _ = new MailAddress(trimmed);
        }
        catch (FormatException)
        {
            return Result<Email>.Failure(UserErrors.EmailInvalid);
        }

        return Result<Email>.Success(new Email(trimmed));
    }
}
