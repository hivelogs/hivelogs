namespace HiveLogs.Application.Setup.Requests;

public sealed record InitializeSetupRequest(
    string SetupPassword,
    string OrganizationName,
    string AdminName,
    string AdminEmail,
    string AdminPassword);
