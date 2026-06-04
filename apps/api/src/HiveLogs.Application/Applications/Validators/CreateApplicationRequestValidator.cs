using FluentValidation;
using HiveLogs.Application.Applications.Requests;
using HiveLogs.Domain.Applications;

namespace HiveLogs.Application.Applications.Validators;

public sealed class CreateApplicationRequestValidator : AbstractValidator<CreateApplicationRequest>
{
    public CreateApplicationRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithErrorCode("applications.name_required")
            .MinimumLength(ApplicationName.MinLength)
            .WithErrorCode("applications.name_required")
            .MaximumLength(ApplicationName.MaxLength)
            .WithErrorCode("applications.name_too_long");
    }
}
