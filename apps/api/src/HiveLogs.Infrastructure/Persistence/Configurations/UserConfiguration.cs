using HiveLogs.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HiveLogs.Infrastructure.Persistence.Configurations;

internal sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users");

        builder.HasKey(u => u.Id);

        builder.Property(u => u.Id)
            .HasColumnName("id");

        builder.Property(u => u.Name)
            .HasConversion(
                name => name.Value,
                value => ValueObjectConverters.ToUserName(value))
            .HasColumnName("name")
            .HasMaxLength(UserName.MaxLength)
            .IsRequired();

        builder.Property(u => u.Email)
            .HasConversion(
                email => email.Value,
                value => ValueObjectConverters.ToEmail(value))
            .HasColumnName("email")
            .HasMaxLength(Email.MaxLength)
            .IsRequired();

        builder.Property(u => u.PasswordHash)
            .HasConversion(
                hash => hash.Value,
                value => ValueObjectConverters.ToPasswordHash(value))
            .HasColumnName("password_hash")
            .HasMaxLength(512)
            .IsRequired();

        builder.Property(u => u.Role)
            .HasColumnName("role")
            .HasConversion<string>()
            .HasMaxLength(32)
            .IsRequired();

        builder.Property(u => u.MustChangePassword)
            .HasColumnName("must_change_password");

        builder.Property(u => u.IsActive)
            .HasColumnName("is_active");

        builder.Property(u => u.CreatedAt)
            .HasColumnName("created_at");

        builder.Property(u => u.UpdatedAt)
            .HasColumnName("updated_at");

        builder.Property<string>("EmailLower")
            .HasComputedColumnSql("lower(email)", stored: true)
            .HasColumnName("email_lower");

        builder.HasIndex("EmailLower")
            .IsUnique()
            .HasDatabaseName("ix_users_email_lower");
    }
}
