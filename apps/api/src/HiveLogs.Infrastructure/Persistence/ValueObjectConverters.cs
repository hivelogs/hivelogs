using HiveLogs.Domain.Applications;
using HiveLogs.Domain.Environments;
using HiveLogs.Domain.Organizations;
using HiveLogs.Domain.Users;

namespace HiveLogs.Infrastructure.Persistence;

internal static class ValueObjectConverters
{
    public static OrganizationName ToOrganizationName(string value) =>
        OrganizationName.Create(value).Value!;

    public static ApplicationName ToApplicationName(string value) =>
        ApplicationName.Create(value).Value!;

    public static EnvironmentName ToEnvironmentName(string value) =>
        EnvironmentName.Create(value).Value!;

    public static UserName ToUserName(string value) =>
        UserName.Create(value).Value!;

    public static Email ToEmail(string value) =>
        Email.Create(value).Value!;

    public static PasswordHash ToPasswordHash(string value) =>
        PasswordHash.Create(value).Value!;
}
