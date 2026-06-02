using FluentAssertions;
using HiveLogs.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace HiveLogs.Infrastructure.Tests;

public class HiveLogsDbContextTests
{
    [Fact]
    public void DbContext_ShouldInstantiateWithInMemoryProvider()
    {
        var options = new DbContextOptionsBuilder<HiveLogsDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        using var context = new HiveLogsDbContext(options);

        context.Should().NotBeNull();
        context.Database.IsInMemory().Should().BeTrue();
    }
}
