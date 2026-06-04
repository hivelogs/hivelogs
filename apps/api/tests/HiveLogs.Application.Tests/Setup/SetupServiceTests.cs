using FluentAssertions;
using HiveLogs.Application.Setup;
using HiveLogs.Application.Setup.Requests;
using HiveLogs.Application.Setup.Validators;
using HiveLogs.Application.Tests.TestDoubles;
using HiveLogs.Domain.OrganizationMembers;
using HiveLogs.Domain.Setup;

namespace HiveLogs.Application.Tests.Setup;

public class SetupServiceTests
{
    private static readonly DateTimeOffset Now = DateTimeOffset.Parse("2026-06-04T12:00:00Z");

    private static SetupService CreateSut(
        InMemorySetupStateRepository? setupRepo = null,
        InMemoryOrganizationRepository? orgRepo = null,
        InMemoryUserRepository? userRepo = null,
        InMemoryOrganizationMemberRepository? memberRepo = null,
        FakeUnitOfWork? unitOfWork = null,
        FakeSetupPasswordValidator? setupPasswordValidator = null,
        FakePasswordHasher? passwordHasher = null)
    {
        setupRepo ??= new InMemorySetupStateRepository();
        orgRepo ??= new InMemoryOrganizationRepository();
        userRepo ??= new InMemoryUserRepository();
        memberRepo ??= new InMemoryOrganizationMemberRepository();
        unitOfWork ??= new FakeUnitOfWork();

        return new SetupService(
            setupRepo,
            orgRepo,
            userRepo,
            memberRepo,
            unitOfWork,
            new FakeClock(Now),
            passwordHasher ?? new FakePasswordHasher(),
            setupPasswordValidator ?? new FakeSetupPasswordValidator(),
            new InitializeSetupRequestValidator());
    }

    [Fact]
    public async Task GetStatusAsync_WhenSetupNotCompleted_ShouldReturnSetupRequired()
    {
        var sut = CreateSut();

        var result = await sut.GetStatusAsync();

        result.IsSuccess.Should().BeTrue();
        result.Value.Status.Should().Be(SetupStatus.SetupRequired);
        result.Value.SetupRequired.Should().BeTrue();
    }

    [Fact]
    public async Task GetStatusAsync_WhenSetupCompleted_ShouldReturnConfigured()
    {
        var setupRepo = new InMemorySetupStateRepository();
        var state = SetupState.CreatePending();
        state.MarkCompleted(Now);
        await setupRepo.AddAsync(state);

        var sut = CreateSut(setupRepo);

        var result = await sut.GetStatusAsync();

        result.IsSuccess.Should().BeTrue();
        result.Value.Status.Should().Be(SetupStatus.Configured);
        result.Value.SetupRequired.Should().BeFalse();
    }

    [Fact]
    public async Task InitializeAsync_WithValidData_ShouldCreateEntities()
    {
        var setupRepo = new InMemorySetupStateRepository();
        var orgRepo = new InMemoryOrganizationRepository();
        var userRepo = new InMemoryUserRepository();
        var memberRepo = new InMemoryOrganizationMemberRepository();
        var passwordHasher = new FakePasswordHasher();
        var sut = CreateSut(setupRepo, orgRepo, userRepo, memberRepo, passwordHasher: passwordHasher);

        var request = new InitializeSetupRequest(
            "setup-secret",
            "Acme Corp",
            "Admin User",
            "admin@acme.com",
            "StrongPass123");

        var result = await sut.InitializeAsync(request);

        result.IsSuccess.Should().BeTrue();
        result.Value.SetupCompleted.Should().BeTrue();
        result.Value.Organization.Name.Should().Be("Acme Corp");
        result.Value.AdminUser.Email.Should().Be("admin@acme.com");
        result.Value.AdminUser.Role.Should().Be("Admin");
        result.Value.AdminUser.MustChangePassword.Should().BeFalse();

        orgRepo.Organizations.Should().ContainSingle();
        userRepo.Users.Should().ContainSingle();
        memberRepo.Members.Should().ContainSingle().Which.Role.Should().Be(OrganizationMemberRole.Owner);

        var storedUser = userRepo.Users.Single();
        storedUser.PasswordHash.Value.Should().Be("hashed:StrongPass123");
        storedUser.PasswordHash.Value.Should().NotBe("StrongPass123");

        (await sut.GetStatusAsync()).Value.Status.Should().Be(SetupStatus.Configured);
    }

    [Fact]
    public async Task InitializeAsync_ResponseMustNotContainPasswordFields()
    {
        var sut = CreateSut();
        var result = await sut.InitializeAsync(new InitializeSetupRequest(
            "setup-secret",
            "Acme",
            "Admin",
            "admin@acme.com",
            "StrongPass123"));

        var json = System.Text.Json.JsonSerializer.Serialize(result.Value);
        json.Should().NotContain("StrongPass123");
        json.Should().NotContain("setup-secret");
        json.Should().NotContain("passwordHash");
    }

    [Fact]
    public async Task InitializeAsync_WithInvalidSetupPassword_ShouldReturnUnauthorized()
    {
        var sut = CreateSut(setupPasswordValidator: new FakeSetupPasswordValidator("expected"));

        var result = await sut.InitializeAsync(new InitializeSetupRequest(
            "wrong",
            "Acme",
            "Admin",
            "admin@acme.com",
            "StrongPass123"));

        result.IsFailure.Should().BeTrue();
        result.Error!.Code.Should().Be("setup.invalid_setup_password");
    }

    [Fact]
    public async Task InitializeAsync_WhenSetupPasswordNotConfigured_ShouldFail()
    {
        var sut = CreateSut(setupPasswordValidator: new FakeSetupPasswordValidator(null));

        var result = await sut.InitializeAsync(new InitializeSetupRequest(
            "any",
            "Acme",
            "Admin",
            "admin@acme.com",
            "StrongPass123"));

        result.IsFailure.Should().BeTrue();
        result.Error!.Code.Should().Be("setup.password_not_configured");
    }

    [Fact]
    public async Task InitializeAsync_WhenAlreadyCompleted_ShouldReturnConflict()
    {
        var setupRepo = new InMemorySetupStateRepository();
        var state = SetupState.CreatePending();
        state.MarkCompleted(Now);
        await setupRepo.AddAsync(state);

        var sut = CreateSut(setupRepo);

        var result = await sut.InitializeAsync(new InitializeSetupRequest(
            "setup-secret",
            "Acme",
            "Admin",
            "admin@acme.com",
            "StrongPass123"));

        result.IsFailure.Should().BeTrue();
        result.Error!.Code.Should().Be("setup.already_completed");
    }

    [Fact]
    public async Task InitializeAsync_WithWeakPassword_ShouldReturnValidationError()
    {
        var sut = CreateSut();

        var result = await sut.InitializeAsync(new InitializeSetupRequest(
            "setup-secret",
            "Acme",
            "Admin",
            "admin@acme.com",
            "weak"));

        result.IsFailure.Should().BeTrue();
        result.Error!.Code.Should().Be("users.password_too_weak");
    }
}
