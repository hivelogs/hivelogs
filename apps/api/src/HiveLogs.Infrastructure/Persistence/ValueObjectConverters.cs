using HiveLogs.Domain.Applications;
using HiveLogs.Domain.Environments;
using HiveLogs.Domain.Organizations;

namespace HiveLogs.Infrastructure.Persistence;

internal static class ValueObjectConverters
{
    public static OrganizationName ToOrganizationName(string value) =>
        OrganizationName.Create(value).Value!;

    public static ApplicationName ToApplicationName(string value) =>
        ApplicationName.Create(value).Value!;

    public static EnvironmentName ToEnvironmentName(string value) =>
        EnvironmentName.Create(value).Value!;
}
