using FluentAssertions;
using HiveLogs.Application.Environments;
using HiveLogs.Application.Environments.Requests;
using HiveLogs.Application.Environments.Validators;
using HiveLogs.Application.Tests.TestDoubles;
using HiveLogs.Domain.Applications;
using HiveLogs.Domain.Organizations;

namespace HiveLogs.Application.Tests.Environments;

public class EnvironmentServiceTests
{
    private static readonly DateTimeOffset Now = DateTimeOffset.Parse("2026-06-02T12:00:00Z");

    private sealed class TestContext
    {
        public InMemoryOrganizationRepository OrganizationRepository { get; } = new();
        public InMemoryApplicationRepository ApplicationRepository { get; } = new();
        public InMemoryEnvironmentRepository EnvironmentRepository { get; }
        public FakeUnitOfWork UnitOfWork { get; } = new();
        public Guid OrganizationId { get; private set; }
        public Guid ApplicationId { get; private set; }

        public TestContext()
        {
            EnvironmentRepository = new InMemoryEnvironmentRepository(ApplicationRepository);
        }

        public async Task SeedApplicationAsync()
        {
            var orgName = OrganizationName.Create("Acme Corp").Value;
            var org = Organization.Create(orgName, Now).Value;
            await OrganizationRepository.AddAsync(org);
            OrganizationId = org.Id;

            var appName = ApplicationName.Create("Web API").Value;
            var app = MonitoredApplication.Create(OrganizationId, appName, Now).Value;
            await ApplicationRepository.AddAsync(app);
            ApplicationId = app.Id;
        }

        public EnvironmentService CreateSut() =>
            new(
                OrganizationRepository,
                EnvironmentRepository,
                UnitOfWork,
                new FakeClock(Now),
                new CreateEnvironmentRequestValidator());
    }

    [Fact]
    public async Task CreateAsync_ShouldNormalizeNameToLowercase()
    {
        var context = new TestContext();
        await context.SeedApplicationAsync();
        var sut = context.CreateSut();

        var result = await sut.CreateAsync(
            context.OrganizationId,
            context.ApplicationId,
            new CreateEnvironmentRequest("PRODUCTION"));

        result.IsSuccess.Should().BeTrue();
        result.Value.Name.Should().Be("production");
        context.UnitOfWork.SaveChangesCallCount.Should().Be(1);
    }

    [Fact]
    public async Task CreateAsync_WhenApplicationNotInOrganization_ShouldReturnApplicationNotFound()
    {
        var context = new TestContext();
        await context.SeedApplicationAsync();
        var sut = context.CreateSut();

        var result = await sut.CreateAsync(
            context.OrganizationId,
            Guid.NewGuid(),
            new CreateEnvironmentRequest("production"));

        result.IsFailure.Should().BeTrue();
        result.Error!.Code.Should().Be("environments.application_not_found");
    }

    [Fact]
    public async Task CreateAsync_WithDuplicateName_ShouldReturnConflict()
    {
        var context = new TestContext();
        await context.SeedApplicationAsync();
        var sut = context.CreateSut();

        await sut.CreateAsync(
            context.OrganizationId,
            context.ApplicationId,
            new CreateEnvironmentRequest("production"));

        var result = await sut.CreateAsync(
            context.OrganizationId,
            context.ApplicationId,
            new CreateEnvironmentRequest("production"));

        result.IsFailure.Should().BeTrue();
        result.Error!.Code.Should().Be("environments.name_already_exists");
    }
}
