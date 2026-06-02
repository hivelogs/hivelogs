using FluentAssertions;
using HiveLogs.Domain.Applications;
using DomainEnvironment = HiveLogs.Domain.Environments.Environment;
using HiveLogs.Domain.Environments;
using HiveLogs.Domain.Organizations;
using HiveLogs.Infrastructure.Persistence;
using HiveLogs.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace HiveLogs.Infrastructure.Tests;

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
        await _dbContext.Database.ExecuteSqlRawAsync(
            "TRUNCATE TABLE environments, applications, organizations RESTART IDENTITY CASCADE");

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

    public async Task DisposeAsync() => await _dbContext.DisposeAsync();

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
