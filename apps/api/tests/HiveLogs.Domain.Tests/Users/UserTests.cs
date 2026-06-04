using FluentAssertions;
using HiveLogs.Domain.Users;

namespace HiveLogs.Domain.Tests.Users;

public class UserTests
{
    private static readonly DateTimeOffset Now = DateTimeOffset.Parse("2026-06-04T12:00:00Z");

    [Fact]
    public void CreateAdminForInitialSetup_WithValidData_ShouldSucceed()
    {
        var name = UserName.Create("Admin User").Value!;
        var email = Email.Create("admin@example.com").Value!;
        var hash = PasswordHash.Create("hashed-password").Value!;

        var result = User.CreateAdminForInitialSetup(name, email, hash, Now);

        result.IsSuccess.Should().BeTrue();
        result.Value!.Role.Should().Be(UserRole.Admin);
        result.Value.MustChangePassword.Should().BeFalse();
        result.Value.IsActive.Should().BeTrue();
    }

    [Fact]
    public void CreateAdminForInitialSetup_CanSetMustChangePasswordTrue()
    {
        var name = UserName.Create("Admin").Value!;
        var email = Email.Create("admin@example.com").Value!;
        var hash = PasswordHash.Create("hash").Value!;

        var result = User.CreateAdminForInitialSetup(name, email, hash, Now, mustChangePassword: true);

        result.Value!.MustChangePassword.Should().BeTrue();
    }

    [Fact]
    public void Email_Create_WithInvalidFormat_ShouldFail()
    {
        var result = Email.Create("not-an-email");

        result.IsFailure.Should().BeTrue();
        result.Error!.Code.Should().Be("users.email_invalid");
    }

    [Fact]
    public void UserName_Create_WithEmptyName_ShouldFail()
    {
        var result = UserName.Create("   ");

        result.IsFailure.Should().BeTrue();
        result.Error!.Code.Should().Be("users.name_required");
    }

    [Fact]
    public void PasswordHash_Create_WithEmptyHash_ShouldFail()
    {
        var result = PasswordHash.Create("");

        result.IsFailure.Should().BeTrue();
        result.Error!.Code.Should().Be("users.password_hash_required");
    }

    [Fact]
    public void Create_WithInvalidRole_ShouldFail()
    {
        var name = UserName.Create("User").Value!;
        var email = Email.Create("user@example.com").Value!;
        var hash = PasswordHash.Create("hash").Value!;

        var result = User.Create(name, email, hash, (UserRole)99, true, Now);

        result.IsFailure.Should().BeTrue();
        result.Error!.Code.Should().Be("users.invalid_role");
    }
}
