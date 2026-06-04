using FluentValidation;
using HiveLogs.Application.Abstractions.Persistence;
using HiveLogs.Application.Abstractions.Time;
using HiveLogs.Application.Common;
using HiveLogs.Application.Organizations.Requests;
using HiveLogs.Application.Organizations.Responses;
using HiveLogs.Domain.Common.Errors;
using HiveLogs.Domain.Organizations;

namespace HiveLogs.Application.Organizations;

public sealed class OrganizationService : IOrganizationService
{
    private readonly IOrganizationRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IClock _clock;
    private readonly IValidator<CreateOrganizationRequest> _createValidator;

    public OrganizationService(
        IOrganizationRepository repository,
        IUnitOfWork unitOfWork,
        IClock clock,
        IValidator<CreateOrganizationRequest> createValidator)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _clock = clock;
        _createValidator = createValidator;
    }

    public async Task<Result<OrganizationResponse>> CreateAsync(
        CreateOrganizationRequest request,
        CancellationToken cancellationToken = default)
    {
        var validation = await _createValidator.ValidateAsync(request, cancellationToken);
        if (!validation.IsValid)
            return validation.ToFailure<OrganizationResponse>();

        if (await _repository.ExistsByNameAsync(request.Name, cancellationToken))
            return Result<OrganizationResponse>.Failure(OrganizationErrors.NameAlreadyExists);

        var nameResult = OrganizationName.Create(request.Name);
        if (nameResult.IsFailure)
            return Result<OrganizationResponse>.Failure(nameResult.Error!);

        var createResult = Organization.Create(nameResult.Value, _clock.UtcNow);
        if (createResult.IsFailure)
            return Result<OrganizationResponse>.Failure(createResult.Error!);

        await _repository.AddAsync(createResult.Value, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<OrganizationResponse>.Success(Map(createResult.Value));
    }

    public async Task<Result<OrganizationResponse>> GetByIdAsync(
        Guid organizationId,
        CancellationToken cancellationToken = default)
    {
        var organization = await _repository.GetByIdAsync(organizationId, cancellationToken);
        if (organization is null)
            return Result<OrganizationResponse>.Failure(OrganizationErrors.NotFound);

        return Result<OrganizationResponse>.Success(Map(organization));
    }

    public async Task<Result<IReadOnlyList<OrganizationResponse>>> ListAsync(
        CancellationToken cancellationToken = default)
    {
        var organizations = await _repository.ListAsync(cancellationToken);
        var responses = organizations.Select(Map).ToList();
        return Result<IReadOnlyList<OrganizationResponse>>.Success(responses);
    }

    private static OrganizationResponse Map(Organization organization) =>
        new(
            organization.Id,
            organization.Name.Value,
            organization.CreatedAt,
            organization.UpdatedAt);
}
