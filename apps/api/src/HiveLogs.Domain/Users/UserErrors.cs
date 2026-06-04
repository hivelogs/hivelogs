using HiveLogs.Domain.Common.Errors;

namespace HiveLogs.Domain.Users;

public static class UserErrors
{
    public static Error NameRequired =>
        Error.Validation("users.name_required", "User name is required.");

    public static Error NameTooLong =>
        Error.Validation("users.name_too_long", "User name exceeds the maximum length.");

    public static Error EmailRequired =>
        Error.Validation("users.email_required", "Email is required.");

    public static Error EmailInvalid =>
        Error.Validation("users.email_invalid", "Email format is invalid.");

    public static Error EmailTooLong =>
        Error.Validation("users.email_too_long", "Email exceeds the maximum length.");

    public static Error EmailAlreadyExists =>
        Error.Conflict("users.email_already_exists", "A user with this email already exists.");

    public static Error PasswordRequired =>
        Error.Validation("users.password_required", "Password is required.");

    public static Error PasswordTooWeak =>
        Error.Validation("users.password_too_weak", "Password must be at least 8 characters and contain at least one letter and one number.");

    public static Error PasswordHashRequired =>
        Error.Validation("users.password_hash_required", "Password hash is required.");

    public static Error InvalidRole =>
        Error.Validation("users.invalid_role", "User role is not valid.");

    public static Error NotFound =>
        Error.NotFound("users.not_found", "User was not found.");
}
