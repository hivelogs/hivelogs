using HiveLogs.Domain.Common.Entities;
using HiveLogs.Domain.Common.Errors;

namespace HiveLogs.Domain.Users;

public sealed class User : AuditableEntity
{
    public UserName Name { get; private set; } = null!;

    public Email Email { get; private set; } = null!;

    public PasswordHash PasswordHash { get; private set; } = null!;

    public UserRole Role { get; private set; }

    public bool MustChangePassword { get; private set; }

    public bool IsActive { get; private set; }

    private User()
    {
    }

    public static Result<User> CreateAdminForInitialSetup(
        UserName name,
        Email email,
        PasswordHash passwordHash,
        DateTimeOffset now,
        bool mustChangePassword = false)
    {
        if (!Enum.IsDefined(typeof(UserRole), UserRole.Admin))
            return Result<User>.Failure(UserErrors.InvalidRole);

        var user = new User
        {
            Id = Guid.NewGuid(),
            Name = name,
            Email = email,
            PasswordHash = passwordHash,
            Role = UserRole.Admin,
            MustChangePassword = mustChangePassword,
            IsActive = true,
            CreatedAt = now,
            UpdatedAt = now
        };

        return Result<User>.Success(user);
    }

    public static Result<User> Create(
        UserName name,
        Email email,
        PasswordHash passwordHash,
        UserRole role,
        bool mustChangePassword,
        DateTimeOffset now)
    {
        if (!Enum.IsDefined(typeof(UserRole), role))
            return Result<User>.Failure(UserErrors.InvalidRole);

        var user = new User
        {
            Id = Guid.NewGuid(),
            Name = name,
            Email = email,
            PasswordHash = passwordHash,
            Role = role,
            MustChangePassword = mustChangePassword,
            IsActive = true,
            CreatedAt = now,
            UpdatedAt = now
        };

        return Result<User>.Success(user);
    }
}
