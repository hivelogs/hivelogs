using FluentValidation;
using HiveLogs.Application.Organizations.Requests;
using HiveLogs.Domain.Organizations;

namespace HiveLogs.Application.Organizations.Validators;

public sealed class CreateOrganizationRequestValidator : AbstractValidator<CreateOrganizationRequest>
{
    public CreateOrganizationRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithErrorCode("organizations.name_required")
            .MinimumLength(OrganizationName.MinLength)
            .WithErrorCode("organizations.name_required")
            .MaximumLength(OrganizationName.MaxLength)
            .WithErrorCode("organizations.name_too_long");
    }
}
