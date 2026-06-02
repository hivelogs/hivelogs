using DomainEnvironment = HiveLogs.Domain.Environments.Environment;
using HiveLogs.Domain.Applications;
using HiveLogs.Domain.Organizations;
using Microsoft.EntityFrameworkCore;

namespace HiveLogs.Infrastructure.Persistence;

public sealed class HiveLogsDbContext : DbContext
{
    public HiveLogsDbContext(DbContextOptions<HiveLogsDbContext> options)
        : base(options)
    {
    }

    public DbSet<Organization> Organizations => Set<Organization>();

    public DbSet<MonitoredApplication> Applications => Set<MonitoredApplication>();

    public DbSet<DomainEnvironment> Environments => Set<DomainEnvironment>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(HiveLogsDbContext).Assembly);
    }
}
