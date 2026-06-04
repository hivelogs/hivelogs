using FluentValidation;
using HiveLogs.Application.Abstractions.Persistence;
using HiveLogs.Application.Abstractions.Time;
using HiveLogs.Application.Applications.Requests;
using HiveLogs.Application.Applications.Responses;
using HiveLogs.Application.Common;
using HiveLogs.Domain.Applications;
using HiveLogs.Domain.Common.Errors;
using HiveLogs.Domain.Organizations;

namespace HiveLogs.Application.Applications;

public sealed class ApplicationService : IApplicationService
{
    private readonly IOrganizationRepository _organizationRepository;
    private readonly IApplicationRepository _applicationRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IClock _clock;
    private readonly IValidator<CreateApplicationRequest> _createValidator;

    public ApplicationService(
        IOrganizationRepository organizationRepository,
        IApplicationRepository applicationRepository,
        IUnitOfWork unitOfWork,
        IClock clock,
        IValidator<CreateApplicationRequest> createValidator)
    {
        _organizationRepository = organizationRepository;
        _applicationRepository = applicationRepository;
        _unitOfWork = unitOfWork;
        _clock = clock;
        _createValidator = createValidator;
    }

    public async Task<Result<ApplicationResponse>> CreateAsync(
        Guid organizationId,
        CreateApplicationRequest request,
        CancellationToken cancellationToken = default)
    {
        var validation = await _createValidator.ValidateAsync(request, cancellationToken);
        if (!validation.IsValid)
            return validation.ToFailure<ApplicationResponse>();

        if (await _organizationRepository.GetByIdAsync(organizationId, cancellationToken) is null)
            return Result<ApplicationResponse>.Failure(ApplicationErrors.OrganizationNotFound);

        if (await _applicationRepository.ExistsByNameInOrganizationAsync(
                organizationId,
                request.Name,
                cancellationToken))
            return Result<ApplicationResponse>.Failure(ApplicationErrors.NameAlreadyExists);

        var nameResult = ApplicationName.Create(request.Name);
        if (nameResult.IsFailure)
            return Result<ApplicationResponse>.Failure(nameResult.Error!);

        var createResult = MonitoredApplication.Create(organizationId, nameResult.Value, _clock.UtcNow);
        if (createResult.IsFailure)
            return Result<ApplicationResponse>.Failure(createResult.Error!);

        await _applicationRepository.AddAsync(createResult.Value, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<ApplicationResponse>.Success(Map(createResult.Value));
    }

    public async Task<Result<ApplicationResponse>> GetByIdAsync(
        Guid organizationId,
        Guid applicationId,
        CancellationToken cancellationToken = default)
    {
        if (await _organizationRepository.GetByIdAsync(organizationId, cancellationToken) is null)
            return Result<ApplicationResponse>.Failure(ApplicationErrors.OrganizationNotFound);

        if (!await _applicationRepository.BelongsToOrganizationAsync(
                organizationId,
                applicationId,
                cancellationToken))
            return Result<ApplicationResponse>.Failure(ApplicationErrors.NotFound);

        var application = await _applicationRepository.GetByIdAsync(applicationId, cancellationToken);
        if (application is null)
            return Result<ApplicationResponse>.Failure(ApplicationErrors.NotFound);

        return Result<ApplicationResponse>.Success(Map(application));
    }

    public async Task<Result<IReadOnlyList<ApplicationResponse>>> ListByOrganizationAsync(
        Guid organizationId,
        CancellationToken cancellationToken = default)
    {
        if (await _organizationRepository.GetByIdAsync(organizationId, cancellationToken) is null)
            return Result<IReadOnlyList<ApplicationResponse>>.Failure(ApplicationErrors.OrganizationNotFound);

        var applications = await _applicationRepository.ListByOrganizationAsync(
            organizationId,
            cancellationToken);

        var responses = applications.Select(Map).ToList();
        return Result<IReadOnlyList<ApplicationResponse>>.Success(responses);
    }

    private static ApplicationResponse Map(MonitoredApplication application) =>
        new(
            application.Id,
            application.OrganizationId,
            application.Name.Value,
            application.CreatedAt,
            application.UpdatedAt);
}
