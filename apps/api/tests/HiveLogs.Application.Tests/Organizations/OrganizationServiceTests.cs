using FluentAssertions;
using HiveLogs.Application.Organizations;
using HiveLogs.Application.Organizations.Requests;
using HiveLogs.Application.Organizations.Validators;
using HiveLogs.Application.Tests.TestDoubles;
using HiveLogs.Domain.Organizations;

namespace HiveLogs.Application.Tests.Organizations;

public class OrganizationServiceTests
{
    private static readonly DateTimeOffset Now = DateTimeOffset.Parse("2026-06-02T12:00:00Z");

    private static OrganizationService CreateSut(
        InMemoryOrganizationRepository? repository = null,
        FakeUnitOfWork? unitOfWork = null)
    {
        repository ??= new InMemoryOrganizationRepository();
        unitOfWork ??= new FakeUnitOfWork();

        return new OrganizationService(
            repository,
            unitOfWork,
            new FakeClock(Now),
            new CreateOrganizationRequestValidator());
    }

    [Fact]
    public async Task CreateAsync_WithValidRequest_ShouldPersistAndReturnResponse()
    {
        var repository = new InMemoryOrganizationRepository();
        var unitOfWork = new FakeUnitOfWork();
        var sut = CreateSut(repository, unitOfWork);

        var result = await sut.CreateAsync(new CreateOrganizationRequest("Acme Corp"));

        result.IsSuccess.Should().BeTrue();
        result.Value.Name.Should().Be("Acme Corp");
        result.Value.CreatedAt.Should().Be(Now);
        unitOfWork.SaveChangesCallCount.Should().Be(1);

        var listed = await sut.ListAsync();
        listed.IsSuccess.Should().BeTrue();
        listed.Value.Should().ContainSingle().Which.Id.Should().Be(result.Value.Id);
    }

    [Fact]
    public async Task CreateAsync_WithDuplicateName_ShouldReturnConflict()
    {
        var repository = new InMemoryOrganizationRepository();
        var sut = CreateSut(repository);

        await sut.CreateAsync(new CreateOrganizationRequest("Acme Corp"));

        var result = await sut.CreateAsync(new CreateOrganizationRequest("acme corp"));

        result.IsFailure.Should().BeTrue();
        result.Error!.Code.Should().Be("organizations.name_already_exists");
    }

    [Theory]
    [InlineData("")]
    [InlineData("A")]
    public async Task CreateAsync_WithInvalidName_ShouldReturnValidationError(string name)
    {
        var sut = CreateSut();

        var result = await sut.CreateAsync(new CreateOrganizationRequest(name));

        result.IsFailure.Should().BeTrue();
        result.Error!.Code.Should().Be("organizations.name_required");
    }

    [Fact]
    public async Task GetByIdAsync_WhenExists_ShouldReturnOrganization()
    {
        var repository = new InMemoryOrganizationRepository();
        var sut = CreateSut(repository);
        var created = await sut.CreateAsync(new CreateOrganizationRequest("Acme Corp"));

        var result = await sut.GetByIdAsync(created.Value.Id);

        result.IsSuccess.Should().BeTrue();
        result.Value.Name.Should().Be("Acme Corp");
    }

    [Fact]
    public async Task GetByIdAsync_WhenMissing_ShouldReturnNotFound()
    {
        var sut = CreateSut();

        var result = await sut.GetByIdAsync(Guid.NewGuid());

        result.IsFailure.Should().BeTrue();
        result.Error!.Code.Should().Be("organizations.not_found");
    }
}
