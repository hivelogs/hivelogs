using System.Text.RegularExpressions;

namespace HiveLogs.Application.Common;

public static partial class PasswordPolicy
{
    private static readonly Regex HasLetter = LetterRegex();
    private static readonly Regex HasDigit = DigitRegex();

    public const int MinLength = 8;

    public static bool IsStrongEnough(string? password) =>
        !string.IsNullOrEmpty(password)
        && password.Length >= MinLength
        && HasLetter.IsMatch(password)
        && HasDigit.IsMatch(password);

    [GeneratedRegex(@"[A-Za-z]")]
    private static partial Regex LetterRegex();

    [GeneratedRegex(@"\d")]
    private static partial Regex DigitRegex();
}
