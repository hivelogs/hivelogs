using HiveLogs.Domain.Setup;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HiveLogs.Infrastructure.Persistence.Configurations;

internal sealed class SetupStateConfiguration : IEntityTypeConfiguration<SetupState>
{
    public void Configure(EntityTypeBuilder<SetupState> builder)
    {
        builder.ToTable("setup_state");

        builder.HasKey(s => s.Id);

        builder.Property(s => s.Id)
            .HasColumnName("id");

        builder.Property(s => s.IsCompleted)
            .HasColumnName("is_completed");

        builder.Property(s => s.CompletedAt)
            .HasColumnName("completed_at");
    }
}
