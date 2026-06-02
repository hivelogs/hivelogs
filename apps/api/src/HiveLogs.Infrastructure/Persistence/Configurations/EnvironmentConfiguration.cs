using DomainEnvironment = HiveLogs.Domain.Environments.Environment;
using HiveLogs.Domain.Applications;
using HiveLogs.Domain.Environments;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HiveLogs.Infrastructure.Persistence.Configurations;

internal sealed class EnvironmentConfiguration : IEntityTypeConfiguration<DomainEnvironment>
{
    public void Configure(EntityTypeBuilder<DomainEnvironment> builder)
    {
        builder.ToTable("environments");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .HasColumnName("id");

        builder.Property(e => e.ApplicationId)
            .HasColumnName("application_id");

        builder.Property(e => e.Name)
            .HasConversion(
                name => name.Value,
                value => ValueObjectConverters.ToEnvironmentName(value))
            .HasColumnName("name")
            .HasMaxLength(EnvironmentName.MaxLength)
            .IsRequired();

        builder.Property(e => e.CreatedAt)
            .HasColumnName("created_at");

        builder.Property(e => e.UpdatedAt)
            .HasColumnName("updated_at");

        builder.HasOne<MonitoredApplication>()
            .WithMany()
            .HasForeignKey(e => e.ApplicationId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(e => new { e.ApplicationId, e.Name })
            .IsUnique()
            .HasDatabaseName("ix_environments_application_id_name");
    }
}
