using FluentAssertions;
using HiveLogs.Domain.Setup;
using HiveLogs.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace HiveLogs.Infrastructure.Tests.Integration;

[Trait("Category", "Integration")]
public sealed class SetupStatePersistenceTests : IAsyncLifetime
{
    private const string ConnectionString =
        "Host=localhost;Port=5432;Database=hivelogs_infrastructure_test;Username=hivelogs;Password=hivelogs";

    private HiveLogsDbContext _dbContext = null!;

    public async Task InitializeAsync()
    {
        await EnsureDatabaseExistsAsync();

        var options = new DbContextOptionsBuilder<HiveLogsDbContext>()
            .UseNpgsql(ConnectionString)
            .Options;

        _dbContext = new HiveLogsDbContext(options);
        await _dbContext.Database.MigrateAsync();
        await TruncateSetupStateAsync();
    }

    [Fact]
    public async Task SetupState_ShouldBeSingletonByPrimaryKey()
    {
        var first = SetupState.CreatePending();
        await _dbContext.SetupStates.AddAsync(first);
        await _dbContext.SaveChangesAsync();

        var duplicate = SetupState.CreatePending();
        await _dbContext.SetupStates.AddAsync(duplicate);

        var act = async () => await _dbContext.SaveChangesAsync();

        await act.Should().ThrowAsync<DbUpdateException>();
    }

    public async Task DisposeAsync()
    {
        await TruncateSetupStateAsync();
        await _dbContext.DisposeAsync();
    }

    private Task TruncateSetupStateAsync() =>
        _dbContext.Database.ExecuteSqlRawAsync("TRUNCATE TABLE setup_state RESTART IDENTITY CASCADE");

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
