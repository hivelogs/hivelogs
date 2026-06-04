using FluentAssertions;
using HiveLogs.Domain.Applications;
using DomainEnvironment = HiveLogs.Domain.Environments.Environment;
using HiveLogs.Domain.Environments;
using HiveLogs.Domain.Organizations;
using HiveLogs.Infrastructure.Persistence;
using HiveLogs.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace HiveLogs.Infrastructure.Tests.Integration;

[Trait("Category", "Integration")]
public sealed class CoreDomainPersistenceTests : IAsyncLifetime
{
    private const string ConnectionString =
        "Host=localhost;Port=5432;Database=hivelogs_infrastructure_test;Username=hivelogs;Password=hivelogs";

    private static readonly DateTimeOffset Now = DateTimeOffset.Parse("2026-06-02T12:00:00Z");

    private HiveLogsDbContext _dbContext = null!;
    private OrganizationRepository _organizationRepository = null!;
    private ApplicationRepository _applicationRepository = null!;
    private EnvironmentRepository _environmentRepository = null!;
    private UnitOfWork _unitOfWork = null!;

    public async Task InitializeAsync()
    {
        await EnsureDatabaseExistsAsync();

        var options = new DbContextOptionsBuilder<HiveLogsDbContext>()
            .UseNpgsql(ConnectionString)
            .Options;

        _dbContext = new HiveLogsDbContext(options);
        await _dbContext.Database.MigrateAsync();
        await TruncateTablesAsync();

        _organizationRepository = new OrganizationRepository(_dbContext);
        _applicationRepository = new ApplicationRepository(_dbContext);
        _environmentRepository = new EnvironmentRepository(_dbContext);
        _unitOfWork = new UnitOfWork(_dbContext);
    }

    [Fact]
    public async Task ShouldPersistOrganizationApplicationEnvironmentChain()
    {
        var organization = Organization.Create(OrganizationName.Create("Acme Corp").Value, Now).Value;
        await _organizationRepository.AddAsync(organization);

        var application = MonitoredApplication.Create(
            organization.Id,
            ApplicationName.Create("Web API").Value,
            Now).Value;
        await _applicationRepository.AddAsync(application);

        var environment = DomainEnvironment.Create(
            application.Id,
            EnvironmentName.Create("production").Value,
            Now).Value;
        await _environmentRepository.AddAsync(environment);

        await _unitOfWork.SaveChangesAsync();

        var persistedOrganization = await _organizationRepository.GetByIdAsync(organization.Id);
        var persistedApplication = await _applicationRepository.GetByIdAsync(application.Id);
        var persistedEnvironment = await _environmentRepository.GetByIdAsync(environment.Id);

        persistedOrganization.Should().NotBeNull();
        persistedOrganization!.Name.Value.Should().Be("Acme Corp");

        persistedApplication.Should().NotBeNull();
        persistedApplication!.OrganizationId.Should().Be(organization.Id);
        persistedApplication.Name.Value.Should().Be("Web API");

        persistedEnvironment.Should().NotBeNull();
        persistedEnvironment!.ApplicationId.Should().Be(application.Id);
        persistedEnvironment.Name.Value.Should().Be("production");
    }

    [Fact]
    public async Task ShouldThrowWhenOrganizationNameViolatesUniqueConstraint()
    {
        var first = Organization.Create(OrganizationName.Create("Acme Corp").Value, Now).Value;
        var duplicate = Organization.Create(OrganizationName.Create("acme corp").Value, Now).Value;

        await _organizationRepository.AddAsync(first);
        await _unitOfWork.SaveChangesAsync();

        await _organizationRepository.AddAsync(duplicate);

        var act = async () => await _unitOfWork.SaveChangesAsync();

        await act.Should().ThrowAsync<DbUpdateException>();
    }

    [Fact]
    public async Task ExistsByNameAsync_ShouldMatchCaseInsensitively()
    {
        var organization = Organization.Create(OrganizationName.Create("Acme Corp").Value, Now).Value;
        await _organizationRepository.AddAsync(organization);
        await _unitOfWork.SaveChangesAsync();

        var exists = await _organizationRepository.ExistsByNameAsync("acme corp");

        exists.Should().BeTrue();
    }

    [Fact]
    public async Task ExistsByNameInOrganizationAsync_ShouldMatchCaseInsensitively()
    {
        var organization = Organization.Create(OrganizationName.Create("Acme Corp").Value, Now).Value;
        await _organizationRepository.AddAsync(organization);

        var application = MonitoredApplication.Create(
            organization.Id,
            ApplicationName.Create("Web API").Value,
            Now).Value;
        await _applicationRepository.AddAsync(application);
        await _unitOfWork.SaveChangesAsync();

        var exists = await _applicationRepository.ExistsByNameInOrganizationAsync(
            organization.Id,
            "web api");

        exists.Should().BeTrue();
    }

    [Fact]
    public async Task ListAsync_ShouldOrderByNameLowerCaseInsensitively()
    {
        var beta = Organization.Create(OrganizationName.Create("Beta Inc").Value, Now).Value;
        var alpha = Organization.Create(OrganizationName.Create("alpha co").Value, Now).Value;
        var gamma = Organization.Create(OrganizationName.Create("Gamma LLC").Value, Now).Value;

        await _organizationRepository.AddAsync(beta);
        await _organizationRepository.AddAsync(alpha);
        await _organizationRepository.AddAsync(gamma);
        await _unitOfWork.SaveChangesAsync();

        var organizations = await _organizationRepository.ListAsync();

        organizations.Select(o => o.Name.Value).Should().Equal("alpha co", "Beta Inc", "Gamma LLC");
    }

    [Fact]
    public async Task ListByOrganizationAsync_ShouldOrderByNameLowerCaseInsensitively()
    {
        var organization = Organization.Create(OrganizationName.Create("Acme Corp").Value, Now).Value;
        await _organizationRepository.AddAsync(organization);

        var beta = MonitoredApplication.Create(
            organization.Id,
            ApplicationName.Create("Beta App").Value,
            Now).Value;
        var alpha = MonitoredApplication.Create(
            organization.Id,
            ApplicationName.Create("alpha app").Value,
            Now).Value;

        await _applicationRepository.AddAsync(beta);
        await _applicationRepository.AddAsync(alpha);
        await _unitOfWork.SaveChangesAsync();

        var applications = await _applicationRepository.ListByOrganizationAsync(organization.Id);

        applications.Select(a => a.Name.Value).Should().Equal("alpha app", "Beta App");
    }

    [Fact]
    public async Task ExistsByNameInApplicationAsync_ShouldFindPersistedEnvironment()
    {
        var organization = Organization.Create(OrganizationName.Create("Acme Corp").Value, Now).Value;
        await _organizationRepository.AddAsync(organization);

        var application = MonitoredApplication.Create(
            organization.Id,
            ApplicationName.Create("Web API").Value,
            Now).Value;
        await _applicationRepository.AddAsync(application);

        var environment = DomainEnvironment.Create(
            application.Id,
            EnvironmentName.Create("production").Value,
            Now).Value;
        await _environmentRepository.AddAsync(environment);
        await _unitOfWork.SaveChangesAsync();

        var exists = await _environmentRepository.ExistsByNameInApplicationAsync(
            application.Id,
            "production");

        exists.Should().BeTrue();
    }

    public async Task DisposeAsync()
    {
        await TruncateTablesAsync();
        await _dbContext.DisposeAsync();
    }

    private Task TruncateTablesAsync() =>
        _dbContext.Database.ExecuteSqlRawAsync(
            "TRUNCATE TABLE environments, applications, organizations RESTART IDENTITY CASCADE");

    private static async Task EnsureDatabaseExistsAsync()
    {
        var builder = new NpgsqlConnectionStringBuilder(ConnectionString);
        var databaseName = builder.Database!;
        builder.Database = "postgres";

        await using var connection = new NpgsqlConnection(builder.ConnectionString);
        await connection.OpenAsync();

        await using var check = new NpgsqlCommand(
            "SELECT 1 FROM pg_database WHERE datname = @name",
            connection);
        check.Parameters.AddWithValue("name", databaseName);
        var exists = await check.ExecuteScalarAsync() is not null;

        if (!exists)
        {
            await using var create = new NpgsqlCommand(
                $"CREATE DATABASE \"{databaseName}\"",
                connection);
            await create.ExecuteNonQueryAsync();
        }
    }
}
