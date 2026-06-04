using HiveLogs.Domain.OrganizationMembers;
using HiveLogs.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HiveLogs.Infrastructure.Persistence.Configurations;

internal sealed class OrganizationMemberConfiguration : IEntityTypeConfiguration<OrganizationMember>
{
    public void Configure(EntityTypeBuilder<OrganizationMember> builder)
    {
        builder.ToTable("organization_members");

        builder.HasKey(m => m.Id);

        builder.Property(m => m.Id)
            .HasColumnName("id");

        builder.Property(m => m.OrganizationId)
            .HasColumnName("organization_id");

        builder.Property(m => m.UserId)
            .HasColumnName("user_id");

        builder.Property(m => m.Role)
            .HasColumnName("role")
            .HasConversion<string>()
            .HasMaxLength(32)
            .IsRequired();

        builder.Property(m => m.CreatedAt)
            .HasColumnName("created_at");

        builder.HasIndex(m => new { m.OrganizationId, m.UserId })
            .IsUnique()
            .HasDatabaseName("ix_organization_members_org_user");

        builder.HasOne<Domain.Organizations.Organization>()
            .WithMany()
            .HasForeignKey(m => m.OrganizationId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(m => m.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
