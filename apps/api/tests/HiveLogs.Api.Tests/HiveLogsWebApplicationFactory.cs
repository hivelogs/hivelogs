using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;

namespace HiveLogs.Api.Tests;

public sealed class HiveLogsWebApplicationFactory : WebApplicationFactory<Program>
{
    private readonly string _databaseName = Guid.NewGuid().ToString();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseSetting("Testing:UseInMemoryDatabase", "true");
        builder.UseSetting("Testing:InMemoryDatabaseName", _databaseName);
    }
}
