using HiveLogs.Domain.Common.Errors;

namespace HiveLogs.Domain.Setup;

public static class SetupErrors
{
    public static Error AlreadyCompleted =>
        Error.Conflict("setup.already_completed", "Initial setup has already been completed.");

    public static Error PasswordNotConfigured =>
        Error.Failure("setup.password_not_configured", "Setup password is not configured on the server.");

    public static Error InvalidSetupPassword =>
        Error.Unauthorized("setup.invalid_setup_password", "Setup password is invalid.");

    public static Error NotCompleted =>
        Error.Failure("setup.not_completed", "Initial setup has not been completed yet.");

    public static Error InitializationFailed =>
        Error.Failure("setup.initialization_failed", "Initial setup could not be completed.");
}
