namespace HiveLogs.Domain.Common.Errors;

public static class GeneralErrors
{
    public static readonly Error Validation =
        Error.Validation("general.validation", "One or more validation errors occurred.");

    public static readonly Error NotFound =
        Error.NotFound("general.not_found", "The requested resource was not found.");

    public static readonly Error Conflict =
        Error.Conflict("general.conflict", "The request conflicts with the current state.");

    public static readonly Error Unauthorized =
        Error.Unauthorized("general.unauthorized", "Authentication is required.");

    public static readonly Error Forbidden =
        Error.Forbidden("general.forbidden", "You do not have permission to perform this action.");

    public static readonly Error Unexpected =
        Error.Failure("general.unexpected", "An unexpected error occurred.");
}
