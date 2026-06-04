namespace HiveLogs.Application.Setup.Responses;

public sealed record InitializeSetupResponse(
    bool SetupCompleted,
    SetupOrganizationSummary Organization,
    SetupAdminUserSummary AdminUser);

public sealed record SetupOrganizationSummary(Guid Id, string Name);

public sealed record SetupAdminUserSummary(
    Guid Id,
    string Name,
    string Email,
    string Role,
    bool MustChangePassword);
