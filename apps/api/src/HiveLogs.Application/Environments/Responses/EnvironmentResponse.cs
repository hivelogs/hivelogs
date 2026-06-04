namespace HiveLogs.Application.Environments.Responses;

public sealed record EnvironmentResponse(
    Guid Id,
    Guid ApplicationId,
    string Name,
    DateTimeOffset CreatedAt,
    DateTimeOffset UpdatedAt);
