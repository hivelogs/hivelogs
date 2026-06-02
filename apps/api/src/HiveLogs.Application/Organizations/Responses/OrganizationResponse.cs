namespace HiveLogs.Application.Organizations.Responses;

public sealed record OrganizationResponse(
    Guid Id,
    string Name,
    DateTimeOffset CreatedAt,
    DateTimeOffset UpdatedAt);
