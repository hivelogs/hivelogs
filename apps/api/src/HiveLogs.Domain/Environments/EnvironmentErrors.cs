using HiveLogs.Domain.Common.Errors;

namespace HiveLogs.Domain.Environments;

public static class EnvironmentErrors
{
    public static Error NameRequired =>
        Error.Validation("environments.name_required", "Environment name is required.");

    public static Error NameInvalid =>
        Error.Validation("environments.name_invalid", "Environment name format is invalid.");

    public static Error NameTooLong =>
        Error.Validation("environments.name_too_long", "Environment name exceeds the maximum length.");

    public static Error NotFound =>
        Error.NotFound("environments.not_found", "Environment was not found.");

    public static Error ApplicationNotFound =>
        Error.NotFound("environments.application_not_found", "Application was not found.");

    public static Error NameAlreadyExists =>
        Error.Conflict("environments.name_already_exists", "An environment with this name already exists in the application.");
}
