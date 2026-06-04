namespace HiveLogs.Application.Applications.Responses;

public sealed record ApplicationResponse(
    Guid Id,
    Guid OrganizationId,
    string Name,
    DateTimeOffset CreatedAt,
    DateTimeOffset UpdatedAt);
