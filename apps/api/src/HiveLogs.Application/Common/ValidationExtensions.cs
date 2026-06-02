using FluentValidation.Results;
using HiveLogs.Domain.Common.Errors;

namespace HiveLogs.Application.Common;

public static class ValidationExtensions
{
    public static Result ToFailure(this ValidationResult validationResult) =>
        Result.Failure(validationResult.ToValidationError());

    public static Result<T> ToFailure<T>(this ValidationResult validationResult) =>
        Result<T>.Failure(validationResult.ToValidationError());

    public static Error ToValidationError(this ValidationResult validationResult)
    {
        var first = validationResult.Errors.FirstOrDefault();

        if (first is null || string.IsNullOrWhiteSpace(first.ErrorCode))
            return GeneralErrors.Validation;

        return Error.Validation(first.ErrorCode, first.ErrorMessage);
    }
}
