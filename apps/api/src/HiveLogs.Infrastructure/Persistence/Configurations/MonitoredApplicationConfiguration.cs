using HiveLogs.Domain.Applications;
using HiveLogs.Domain.Organizations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HiveLogs.Infrastructure.Persistence.Configurations;

internal sealed class MonitoredApplicationConfiguration : IEntityTypeConfiguration<MonitoredApplication>
{
    public void Configure(EntityTypeBuilder<MonitoredApplication> builder)
    {
        builder.ToTable("applications");

        builder.HasKey(a => a.Id);

        builder.Property(a => a.Id)
            .HasColumnName("id");

        builder.Property(a => a.OrganizationId)
            .HasColumnName("organization_id");

        builder.Property(a => a.Name)
            .HasConversion(
                name => name.Value,
                value => ValueObjectConverters.ToApplicationName(value))
            .HasColumnName("name")
            .HasMaxLength(ApplicationName.MaxLength)
            .IsRequired();

        builder.Property(a => a.CreatedAt)
            .HasColumnName("created_at");

        builder.Property(a => a.UpdatedAt)
            .HasColumnName("updated_at");

        builder.HasOne<Organization>()
            .WithMany()
            .HasForeignKey(a => a.OrganizationId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property<string>("NameLower")
            .HasComputedColumnSql("lower(name)", stored: true)
            .HasColumnName("name_lower");

        builder.HasIndex(nameof(MonitoredApplication.OrganizationId), "NameLower")
            .IsUnique()
            .HasDatabaseName("ix_applications_organization_id_name_lower");
    }
}
