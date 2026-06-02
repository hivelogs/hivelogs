using HiveLogs.Api.Infrastructure.ProblemDetails;
using HiveLogs.Domain.Common.Errors;
using Microsoft.AspNetCore.Mvc;

namespace HiveLogs.Api.Infrastructure.ResultMapping;

public static class ResultExtensions
{
    public static IActionResult ToActionResult(this Result result, HttpContext httpContext)
    {
        if (result.IsSuccess)
            return new NoContentResult();

        return result.Error!.ToProblemDetailsResult(httpContext);
    }

    public static IActionResult ToActionResult<T>(this Result<T> result, HttpContext httpContext)
    {
        if (result.IsSuccess)
            return new OkObjectResult(result.Value);

        return result.Error!.ToProblemDetailsResult(httpContext);
    }

    private static ObjectResult ToProblemDetailsResult(this Error error, HttpContext httpContext)
    {
        var statusCode = error.Type.ToStatusCode();
        var problemDetails = new Microsoft.AspNetCore.Mvc.ProblemDetails
        {
            Type = error.Type.ToTypeUri(),
            Title = error.Type.ToTitle(),
            Status = statusCode,
            Detail = error.Message,
            Extensions =
            {
                ["code"] = error.Code,
                ["traceId"] = httpContext.TraceIdentifier
            }
        };

        return new ObjectResult(problemDetails)
        {
            StatusCode = statusCode,
            ContentTypes = { "application/problem+json" }
        };
    }
}
