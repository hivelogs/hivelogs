using HiveLogs.Domain.Common.Errors;

namespace HiveLogs.Domain.Applications;

public static class ApplicationErrors
{
    public static Error NameRequired =>
        Error.Validation("applications.name_required", "Application name is required.");

    public static Error NameTooLong =>
        Error.Validation("applications.name_too_long", "Application name exceeds the maximum length.");

    public static Error NotFound =>
        Error.NotFound("applications.not_found", "Application was not found.");

    public static Error OrganizationNotFound =>
        Error.NotFound("applications.organization_not_found", "Organization was not found.");

    public static Error NameAlreadyExists =>
        Error.Conflict("applications.name_already_exists", "An application with this name already exists in the organization.");
}
