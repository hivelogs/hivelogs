using FluentValidation;
using HiveLogs.Application.Environments.Requests;
using HiveLogs.Domain.Environments;

namespace HiveLogs.Application.Environments.Validators;

public sealed class CreateEnvironmentRequestValidator : AbstractValidator<CreateEnvironmentRequest>
{
    public CreateEnvironmentRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithErrorCode("environments.name_required")
            .MinimumLength(EnvironmentName.MinLength)
            .WithErrorCode("environments.name_required")
            .MaximumLength(EnvironmentName.MaxLength)
            .WithErrorCode("environments.name_too_long");
    }
}
