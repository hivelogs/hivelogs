using FluentAssertions;
using HiveLogs.Application.Applications;
using HiveLogs.Application.Applications.Requests;
using HiveLogs.Application.Applications.Validators;
using HiveLogs.Application.Tests.TestDoubles;
using HiveLogs.Domain.Applications;
using HiveLogs.Domain.Organizations;

namespace HiveLogs.Application.Tests.Applications;

public class ApplicationServiceTests
{
    private static readonly DateTimeOffset Now = DateTimeOffset.Parse("2026-06-02T12:00:00Z");

    private sealed class TestContext
    {
        public InMemoryOrganizationRepository OrganizationRepository { get; } = new();
        public InMemoryApplicationRepository ApplicationRepository { get; } = new();
        public FakeUnitOfWork UnitOfWork { get; } = new();
        public Guid OrganizationId { get; private set; }

        public async Task SeedOrganizationAsync(string name = "Acme Corp")
        {
            var nameResult = OrganizationName.Create(name);
            var org = Organization.Create(nameResult.Value, Now).Value;
            await OrganizationRepository.AddAsync(org);
            OrganizationId = org.Id;
        }

        public ApplicationService CreateSut() =>
            new(
                OrganizationRepository,
                ApplicationRepository,
                UnitOfWork,
                new FakeClock(Now),
                new CreateApplicationRequestValidator());
    }

    [Fact]
    public async Task CreateAsync_WhenOrganizationExists_ShouldPersistAndReturnResponse()
    {
        var context = new TestContext();
        await context.SeedOrganizationAsync();
        var sut = context.CreateSut();

        var result = await sut.CreateAsync(
            context.OrganizationId,
            new CreateApplicationRequest("Web API"));

        result.IsSuccess.Should().BeTrue();
        result.Value.OrganizationId.Should().Be(context.OrganizationId);
        result.Value.Name.Should().Be("Web API");
        context.UnitOfWork.SaveChangesCallCount.Should().Be(1);
    }

    [Fact]
    public async Task CreateAsync_WhenOrganizationMissing_ShouldReturnOrganizationNotFound()
    {
        var sut = new TestContext().CreateSut();

        var result = await sut.CreateAsync(
            Guid.NewGuid(),
            new CreateApplicationRequest("Web API"));

        result.IsFailure.Should().BeTrue();
        result.Error!.Code.Should().Be("applications.organization_not_found");
    }

    [Fact]
    public async Task CreateAsync_WithDuplicateNameInOrganization_ShouldReturnConflict()
    {
        var context = new TestContext();
        await context.SeedOrganizationAsync();
        var sut = context.CreateSut();

        await sut.CreateAsync(context.OrganizationId, new CreateApplicationRequest("Web API"));

        var result = await sut.CreateAsync(
            context.OrganizationId,
            new CreateApplicationRequest("web api"));

        result.IsFailure.Should().BeTrue();
        result.Error!.Code.Should().Be("applications.name_already_exists");
    }
}
