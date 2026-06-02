using Microsoft.EntityFrameworkCore;

namespace HiveLogs.Infrastructure.Persistence;

public sealed class HiveLogsDbContext : DbContext
{
    public HiveLogsDbContext(DbContextOptions<HiveLogsDbContext> options)
        : base(options)
    {
    }
}
