using HiveLogs.Domain.Common.Errors;

namespace HiveLogs.Domain.Organizations;

public static class OrganizationErrors
{
    public static Error NameRequired =>
        Error.Validation("organizations.name_required", "Organization name is required.");

    public static Error NameTooLong =>
        Error.Validation("organizations.name_too_long", "Organization name exceeds the maximum length.");

    public static Error NotFound =>
        Error.NotFound("organizations.not_found", "Organization was not found.");

    public static Error NameAlreadyExists =>
        Error.Conflict("organizations.name_already_exists", "An organization with this name already exists.");
}
