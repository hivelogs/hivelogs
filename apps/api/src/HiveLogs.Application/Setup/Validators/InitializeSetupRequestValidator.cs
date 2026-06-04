using FluentValidation;
using HiveLogs.Application.Common;
using HiveLogs.Application.Setup.Requests;
using HiveLogs.Domain.Organizations;
using HiveLogs.Domain.Users;

namespace HiveLogs.Application.Setup.Validators;

public sealed class InitializeSetupRequestValidator : AbstractValidator<InitializeSetupRequest>
{
    public InitializeSetupRequestValidator()
    {
        RuleFor(x => x.SetupPassword)
            .NotEmpty()
            .WithErrorCode("setup.setup_password_required")
            .WithMessage("Setup password is required.");

        RuleFor(x => x.OrganizationName)
            .NotEmpty()
            .WithErrorCode(OrganizationErrors.NameRequired.Code)
            .WithMessage(OrganizationErrors.NameRequired.Message)
            .MaximumLength(OrganizationName.MaxLength)
            .WithErrorCode(OrganizationErrors.NameTooLong.Code)
            .WithMessage(OrganizationErrors.NameTooLong.Message)
            .MinimumLength(OrganizationName.MinLength)
            .WithErrorCode(OrganizationErrors.NameRequired.Code)
            .WithMessage(OrganizationErrors.NameRequired.Message);

        RuleFor(x => x.AdminName)
            .NotEmpty()
            .WithErrorCode(UserErrors.NameRequired.Code)
            .WithMessage(UserErrors.NameRequired.Message)
            .MaximumLength(UserName.MaxLength)
            .WithErrorCode(UserErrors.NameTooLong.Code)
            .WithMessage(UserErrors.NameTooLong.Message);

        RuleFor(x => x.AdminEmail)
            .NotEmpty()
            .WithErrorCode(UserErrors.EmailRequired.Code)
            .WithMessage(UserErrors.EmailRequired.Message)
            .EmailAddress()
            .WithErrorCode(UserErrors.EmailInvalid.Code)
            .WithMessage(UserErrors.EmailInvalid.Message)
            .MaximumLength(Email.MaxLength)
            .WithErrorCode(UserErrors.EmailTooLong.Code)
            .WithMessage(UserErrors.EmailTooLong.Message);

        RuleFor(x => x.AdminPassword)
            .NotEmpty()
            .WithErrorCode(UserErrors.PasswordRequired.Code)
            .WithMessage(UserErrors.PasswordRequired.Message)
            .Must(PasswordPolicy.IsStrongEnough)
            .WithErrorCode(UserErrors.PasswordTooWeak.Code)
            .WithMessage(UserErrors.PasswordTooWeak.Message);
    }
}
