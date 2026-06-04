using FluentValidation;
using HiveLogs.Application.Abstractions.Persistence;
using HiveLogs.Application.Abstractions.Security;
using HiveLogs.Application.Abstractions.Time;
using HiveLogs.Application.Common;
using HiveLogs.Application.Setup.Requests;
using HiveLogs.Application.Setup.Responses;
using HiveLogs.Domain.Common.Errors;
using HiveLogs.Domain.OrganizationMembers;
using HiveLogs.Domain.Organizations;
using HiveLogs.Domain.Setup;
using HiveLogs.Domain.Users;

namespace HiveLogs.Application.Setup;

public sealed class SetupService : ISetupService
{
    private readonly ISetupStateRepository _setupStateRepository;
    private readonly IOrganizationRepository _organizationRepository;
    private readonly IUserRepository _userRepository;
    private readonly IOrganizationMemberRepository _organizationMemberRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IClock _clock;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ISetupPasswordValidator _setupPasswordValidator;
    private readonly IValidator<InitializeSetupRequest> _initializeValidator;

    public SetupService(
        ISetupStateRepository setupStateRepository,
        IOrganizationRepository organizationRepository,
        IUserRepository userRepository,
        IOrganizationMemberRepository organizationMemberRepository,
        IUnitOfWork unitOfWork,
        IClock clock,
        IPasswordHasher passwordHasher,
        ISetupPasswordValidator setupPasswordValidator,
        IValidator<InitializeSetupRequest> initializeValidator)
    {
        _setupStateRepository = setupStateRepository;
        _organizationRepository = organizationRepository;
        _userRepository = userRepository;
        _organizationMemberRepository = organizationMemberRepository;
        _unitOfWork = unitOfWork;
        _clock = clock;
        _passwordHasher = passwordHasher;
        _setupPasswordValidator = setupPasswordValidator;
        _initializeValidator = initializeValidator;
    }

    public async Task<Result<SetupStatusResponse>> GetStatusAsync(
        CancellationToken cancellationToken = default)
    {
        var setupState = await EnsureSetupStateAsync(cancellationToken);
        var status = setupState.GetStatus();

        return Result<SetupStatusResponse>.Success(
            new SetupStatusResponse(status, status == SetupStatus.SetupRequired));
    }

    public async Task<Result<InitializeSetupResponse>> InitializeAsync(
        InitializeSetupRequest request,
        CancellationToken cancellationToken = default)
    {
        var validation = await _initializeValidator.ValidateAsync(request, cancellationToken);
        if (!validation.IsValid)
            return validation.ToFailure<InitializeSetupResponse>();

        var setupPasswordResult = _setupPasswordValidator.Validate(request.SetupPassword);
        if (setupPasswordResult.IsFailure)
            return Result<InitializeSetupResponse>.Failure(setupPasswordResult.Error!);

        var setupState = await EnsureSetupStateAsync(cancellationToken);
        if (setupState.IsCompleted)
            return Result<InitializeSetupResponse>.Failure(SetupErrors.AlreadyCompleted);

        if (await _organizationRepository.ExistsByNameAsync(request.OrganizationName, cancellationToken))
            return Result<InitializeSetupResponse>.Failure(OrganizationErrors.NameAlreadyExists);

        if (await _userRepository.ExistsByEmailAsync(request.AdminEmail, cancellationToken))
            return Result<InitializeSetupResponse>.Failure(UserErrors.EmailAlreadyExists);

        var organizationNameResult = OrganizationName.Create(request.OrganizationName);
        if (organizationNameResult.IsFailure)
            return Result<InitializeSetupResponse>.Failure(organizationNameResult.Error!);

        var adminNameResult = UserName.Create(request.AdminName);
        if (adminNameResult.IsFailure)
            return Result<InitializeSetupResponse>.Failure(adminNameResult.Error!);

        var adminEmailResult = Email.Create(request.AdminEmail);
        if (adminEmailResult.IsFailure)
            return Result<InitializeSetupResponse>.Failure(adminEmailResult.Error!);

        var now = _clock.UtcNow;

        var organizationResult = Organization.Create(organizationNameResult.Value, now);
        if (organizationResult.IsFailure)
            return Result<InitializeSetupResponse>.Failure(organizationResult.Error!);

        var passwordHash = PasswordHash.Create(_passwordHasher.Hash(request.AdminPassword));
        if (passwordHash.IsFailure)
            return Result<InitializeSetupResponse>.Failure(passwordHash.Error!);

        var adminResult = User.CreateAdminForInitialSetup(
            adminNameResult.Value,
            adminEmailResult.Value,
            passwordHash.Value,
            now,
            mustChangePassword: false);
        if (adminResult.IsFailure)
            return Result<InitializeSetupResponse>.Failure(adminResult.Error!);

        var membershipResult = OrganizationMember.Create(
            organizationResult.Value.Id,
            adminResult.Value.Id,
            OrganizationMemberRole.Owner,
            now);
        if (membershipResult.IsFailure)
            return Result<InitializeSetupResponse>.Failure(membershipResult.Error!);

        await _organizationRepository.AddAsync(organizationResult.Value, cancellationToken);
        await _userRepository.AddAsync(adminResult.Value, cancellationToken);
        await _organizationMemberRepository.AddAsync(membershipResult.Value, cancellationToken);

        setupState.MarkCompleted(now);
        await _setupStateRepository.UpdateAsync(setupState, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var organization = organizationResult.Value;
        var admin = adminResult.Value;

        return Result<InitializeSetupResponse>.Success(
            new InitializeSetupResponse(
                SetupCompleted: true,
                Organization: new SetupOrganizationSummary(organization.Id, organization.Name.Value),
                AdminUser: new SetupAdminUserSummary(
                    admin.Id,
                    admin.Name.Value,
                    admin.Email.Value,
                    admin.Role.ToString(),
                    admin.MustChangePassword)));
    }

    private async Task<SetupState> EnsureSetupStateAsync(CancellationToken cancellationToken)
    {
        var existing = await _setupStateRepository.GetAsync(cancellationToken);
        if (existing is not null)
            return existing;

        var pending = SetupState.CreatePending();
        await _setupStateRepository.AddAsync(pending, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return pending;
    }
}
