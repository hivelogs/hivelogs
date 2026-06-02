using HiveLogs.Domain.Common.Errors;

namespace HiveLogs.Api.Infrastructure.ProblemDetails;

public static class ErrorTypeStatusCodeMapper
{
    public static int ToStatusCode(this ErrorType errorType) => errorType switch
    {
        ErrorType.Validation => StatusCodes.Status400BadRequest,
        ErrorType.NotFound => StatusCodes.Status404NotFound,
        ErrorType.Conflict => StatusCodes.Status409Conflict,
        ErrorType.Unauthorized => StatusCodes.Status401Unauthorized,
        ErrorType.Forbidden => StatusCodes.Status403Forbidden,
        ErrorType.Failure => StatusCodes.Status500InternalServerError,
        _ => StatusCodes.Status500InternalServerError
    };

    public static string ToTitle(this ErrorType errorType) => errorType switch
    {
        ErrorType.Validation => "Validation",
        ErrorType.NotFound => "Not Found",
        ErrorType.Conflict => "Conflict",
        ErrorType.Unauthorized => "Unauthorized",
        ErrorType.Forbidden => "Forbidden",
        ErrorType.Failure => "Unexpected error",
        _ => "Unexpected error"
    };

    public static string ToTypeUri(this ErrorType errorType) => errorType switch
    {
        ErrorType.Validation => "https://docs.hivelogs.dev/errors/validation",
        ErrorType.NotFound => "https://docs.hivelogs.dev/errors/not-found",
        ErrorType.Conflict => "https://docs.hivelogs.dev/errors/conflict",
        ErrorType.Unauthorized => "https://docs.hivelogs.dev/errors/unauthorized",
        ErrorType.Forbidden => "https://docs.hivelogs.dev/errors/forbidden",
        ErrorType.Failure => "https://docs.hivelogs.dev/errors/unexpected",
        _ => "https://docs.hivelogs.dev/errors/unexpected"
    };
}
