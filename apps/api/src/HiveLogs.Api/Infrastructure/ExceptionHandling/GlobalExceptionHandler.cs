using HiveLogs.Domain.Common.Errors;
using HiveLogs.Domain.Common.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace HiveLogs.Api.Infrastructure.ExceptionHandling;

public sealed class GlobalExceptionHandler : IExceptionHandler
{
    private readonly IHostEnvironment _environment;
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(
        IHostEnvironment environment,
        ILogger<GlobalExceptionHandler> logger)
    {
        _environment = environment;
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        _logger.LogError(exception, "Unhandled exception occurred.");

        var (statusCode, title, type, detail, code) = MapException(exception);

        var problemDetails = new Microsoft.AspNetCore.Mvc.ProblemDetails
        {
            Type = type,
            Title = title,
            Status = statusCode,
            Detail = detail,
            Extensions =
            {
                ["code"] = code,
                ["traceId"] = httpContext.TraceIdentifier
            }
        };

        httpContext.Response.StatusCode = statusCode;
        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
        return true;
    }

    internal (int StatusCode, string Title, string Type, string Detail, string Code) MapException(
        Exception exception)
    {
        if (exception is DomainException domainException)
        {
            return (
                StatusCodes.Status400BadRequest,
                "Domain error",
                "https://docs.hivelogs.dev/errors/domain",
                domainException.Message,
                "general.validation");
        }

        var detail = _environment.IsDevelopment()
            ? exception.Message
            : GeneralErrors.Unexpected.Message;

        return (
            StatusCodes.Status500InternalServerError,
            "Unexpected error",
            "https://docs.hivelogs.dev/errors/unexpected",
            detail,
            GeneralErrors.Unexpected.Code);
    }
}
