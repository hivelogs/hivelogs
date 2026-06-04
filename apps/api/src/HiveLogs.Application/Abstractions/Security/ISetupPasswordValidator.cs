using HiveLogs.Domain.Common.Errors;

namespace HiveLogs.Application.Abstractions.Security;

public interface ISetupPasswordValidator
{
    Result Validate(string? setupPassword);
}
