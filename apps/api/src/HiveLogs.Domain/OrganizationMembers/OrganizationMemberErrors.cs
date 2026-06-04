using HiveLogs.Domain.Common.Errors;

namespace HiveLogs.Domain.OrganizationMembers;

public static class OrganizationMemberErrors
{
    public static Error InvalidIds =>
        Error.Validation("organization_members.invalid_ids", "Organization and user identifiers are required.");

    public static Error InvalidRole =>
        Error.Validation("organization_members.invalid_role", "Organization member role is not valid.");
}
