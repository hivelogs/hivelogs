using DomainEnvironment = HiveLogs.Domain.Environments.Environment;
using HiveLogs.Domain.Applications;
using HiveLogs.Domain.OrganizationMembers;
using HiveLogs.Domain.Organizations;
using HiveLogs.Domain.Setup;
using HiveLogs.Domain.Users;
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

    public DbSet<User> Users => Set<User>();

    public DbSet<OrganizationMember> OrganizationMembers => Set<OrganizationMember>();

    public DbSet<SetupState> SetupStates => Set<SetupState>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(HiveLogsDbContext).Assembly);
    }
}
