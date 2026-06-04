using HiveLogs.Domain.Setup;

namespace HiveLogs.Application.Setup.Responses;

public sealed record SetupStatusResponse(SetupStatus Status, bool SetupRequired);
