using HiveLogs.Domain.Organizations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HiveLogs.Infrastructure.Persistence.Configurations;

internal sealed class OrganizationConfiguration : IEntityTypeConfiguration<Organization>
{
    public void Configure(EntityTypeBuilder<Organization> builder)
    {
        builder.ToTable("organizations");

        builder.HasKey(o => o.Id);

        builder.Property(o => o.Id)
            .HasColumnName("id");

        builder.Property(o => o.Name)
            .HasConversion(
                name => name.Value,
                value => ValueObjectConverters.ToOrganizationName(value))
            .HasColumnName("name")
            .HasMaxLength(OrganizationName.MaxLength)
            .IsRequired();

        builder.Property(o => o.CreatedAt)
            .HasColumnName("created_at");

        builder.Property(o => o.UpdatedAt)
            .HasColumnName("updated_at");

        builder.Property<string>("NameLower")
            .HasComputedColumnSql("lower(name)", stored: true)
            .HasColumnName("name_lower");

        builder.HasIndex("NameLower")
            .IsUnique()
            .HasDatabaseName("ix_organizations_name_lower");
    }
}
