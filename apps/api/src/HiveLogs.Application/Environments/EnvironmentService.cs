using FluentValidation;
using HiveLogs.Application.Abstractions.Persistence;
using HiveLogs.Application.Abstractions.Time;
using HiveLogs.Application.Common;
using HiveLogs.Application.Environments.Requests;
using HiveLogs.Application.Environments.Responses;
using DomainEnvironment = HiveLogs.Domain.Environments.Environment;
using HiveLogs.Domain.Common.Errors;
using HiveLogs.Domain.Environments;
using HiveLogs.Domain.Applications;
using HiveLogs.Domain.Organizations;

namespace HiveLogs.Application.Environments;

public sealed class EnvironmentService : IEnvironmentService
{
    private readonly IOrganizationRepository _organizationRepository;
    private readonly IEnvironmentRepository _environmentRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IClock _clock;
    private readonly IValidator<CreateEnvironmentRequest> _createValidator;

    public EnvironmentService(
        IOrganizationRepository organizationRepository,
        IEnvironmentRepository environmentRepository,
        IUnitOfWork unitOfWork,
        IClock clock,
        IValidator<CreateEnvironmentRequest> createValidator)
    {
        _organizationRepository = organizationRepository;
        _environmentRepository = environmentRepository;
        _unitOfWork = unitOfWork;
        _clock = clock;
        _createValidator = createValidator;
    }

    public async Task<Result<EnvironmentResponse>> CreateAsync(
        Guid organizationId,
        Guid applicationId,
        CreateEnvironmentRequest request,
        CancellationToken cancellationToken = default)
    {
        var validation = await _createValidator.ValidateAsync(request, cancellationToken);
        if (!validation.IsValid)
            return validation.ToFailure<EnvironmentResponse>();

        if (await _organizationRepository.GetByIdAsync(organizationId, cancellationToken) is null)
            return Result<EnvironmentResponse>.Failure(ApplicationErrors.OrganizationNotFound);

        if (!await _environmentRepository.BelongsToOrganizationAsync(
                organizationId,
                applicationId,
                cancellationToken))
            return Result<EnvironmentResponse>.Failure(EnvironmentErrors.ApplicationNotFound);

        var nameResult = EnvironmentName.Create(request.Name);
        if (nameResult.IsFailure)
            return Result<EnvironmentResponse>.Failure(nameResult.Error!);

        if (await _environmentRepository.ExistsByNameInApplicationAsync(
                applicationId,
                nameResult.Value.Value,
                cancellationToken))
            return Result<EnvironmentResponse>.Failure(EnvironmentErrors.NameAlreadyExists);

        var createResult = DomainEnvironment.Create(applicationId, nameResult.Value, _clock.UtcNow);
        if (createResult.IsFailure)
            return Result<EnvironmentResponse>.Failure(createResult.Error!);

        await _environmentRepository.AddAsync(createResult.Value, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<EnvironmentResponse>.Success(Map(createResult.Value));
    }

    public async Task<Result<EnvironmentResponse>> GetByIdAsync(
        Guid organizationId,
        Guid applicationId,
        Guid environmentId,
        CancellationToken cancellationToken = default)
    {
        if (await _organizationRepository.GetByIdAsync(organizationId, cancellationToken) is null)
            return Result<EnvironmentResponse>.Failure(ApplicationErrors.OrganizationNotFound);

        if (!await _environmentRepository.BelongsToOrganizationAsync(
                organizationId,
                applicationId,
                cancellationToken))
            return Result<EnvironmentResponse>.Failure(EnvironmentErrors.ApplicationNotFound);

        var environment = await _environmentRepository.GetByIdAsync(environmentId, cancellationToken);
        if (environment is null || environment.ApplicationId != applicationId)
            return Result<EnvironmentResponse>.Failure(EnvironmentErrors.NotFound);

        return Result<EnvironmentResponse>.Success(Map(environment));
    }

    public async Task<Result<IReadOnlyList<EnvironmentResponse>>> ListByApplicationAsync(
        Guid organizationId,
        Guid applicationId,
        CancellationToken cancellationToken = default)
    {
        if (await _organizationRepository.GetByIdAsync(organizationId, cancellationToken) is null)
            return Result<IReadOnlyList<EnvironmentResponse>>.Failure(ApplicationErrors.OrganizationNotFound);

        if (!await _environmentRepository.BelongsToOrganizationAsync(
                organizationId,
                applicationId,
                cancellationToken))
            return Result<IReadOnlyList<EnvironmentResponse>>.Failure(EnvironmentErrors.ApplicationNotFound);

        var environments = await _environmentRepository.ListByApplicationAsync(
            applicationId,
            cancellationToken);

        var responses = environments.Select(Map).ToList();
        return Result<IReadOnlyList<EnvironmentResponse>>.Success(responses);
    }

    private static EnvironmentResponse Map(DomainEnvironment environment) =>
        new(
            environment.Id,
            environment.ApplicationId,
            environment.Name.Value,
            environment.CreatedAt,
            environment.UpdatedAt);
}
