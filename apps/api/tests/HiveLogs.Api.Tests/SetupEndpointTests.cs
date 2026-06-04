using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using FluentAssertions;
using HiveLogs.Api.Tests.Support;
using HiveLogs.Application.Setup.Requests;
using HiveLogs.Application.Setup.Responses;
using HiveLogs.Domain.Setup;

namespace HiveLogs.Api.Tests;

public sealed class SetupEndpointTests
{
    private const string SetupPassword = "test-setup-password";

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        Converters = { new JsonStringEnumConverter() }
    };

    [Fact]
    public async Task GetSetupStatus_WhenPending_ShouldReturnSetupRequired()
    {
        await using var factory = new HiveLogsWebApplicationFactory();
        var client = factory.CreateClient();

        var response = await client.GetAsync("/setup/status");

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var body = await response.Content.ReadFromJsonAsync<SetupStatusResponse>(JsonOptions);
        body.Should().NotBeNull();
        body!.Status.Should().Be(SetupStatus.SetupRequired);
        body.SetupRequired.Should().BeTrue();
    }

    [Fact]
    public async Task PostSetupInitialize_WithValidData_ShouldReturn201()
    {
        await using var factory = new HiveLogsWebApplicationFactory();
        var client = factory.CreateClient();

        var response = await client.PostAsJsonAsync(
            "/setup/initialize",
            new InitializeSetupRequest(
                SetupPassword,
                "Acme Corp",
                "Admin",
                "admin@acme.com",
                "StrongPass123"));

        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var raw = await response.Content.ReadAsStringAsync();
        raw.Should().NotContain("StrongPass123");
        raw.Should().NotContain(SetupPassword);
        raw.Should().NotContain("passwordHash");

        var body = JsonSerializer.Deserialize<InitializeSetupResponse>(raw, JsonOptions);
        body.Should().NotBeNull();
        body!.SetupCompleted.Should().BeTrue();
        body.Organization.Name.Should().Be("Acme Corp");
        body.AdminUser.Email.Should().Be("admin@acme.com");
        body.AdminUser.MustChangePassword.Should().BeFalse();
    }

    [Fact]
    public async Task PostSetupInitialize_WithInvalidSetupPassword_ShouldReturn401()
    {
        await using var factory = new HiveLogsWebApplicationFactory();
        var client = factory.CreateClient();

        var response = await client.PostAsJsonAsync(
            "/setup/initialize",
            new InitializeSetupRequest(
                "wrong-password",
                "Acme",
                "Admin",
                "admin@acme.com",
                "StrongPass123"));

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);

        var problem = await ProblemDetailsResponse.ReadAsync(response);
        problem!.Code.Should().Be("setup.invalid_setup_password");
    }

    [Fact]
    public async Task PostSetupInitialize_SecondCall_ShouldReturn409()
    {
        await using var factory = new HiveLogsWebApplicationFactory();
        var client = factory.CreateClient();

        await client.PostAsJsonAsync(
            "/setup/initialize",
            new InitializeSetupRequest(
                SetupPassword,
                "Acme",
                "Admin",
                "admin@acme.com",
                "StrongPass123"));

        var response = await client.PostAsJsonAsync(
            "/setup/initialize",
            new InitializeSetupRequest(
                SetupPassword,
                "Other",
                "Other Admin",
                "other@acme.com",
                "StrongPass456"));

        response.StatusCode.Should().Be(HttpStatusCode.Conflict);

        var problem = await ProblemDetailsResponse.ReadAsync(response);
        problem!.Code.Should().Be("setup.already_completed");
    }

    [Fact]
    public async Task GetSetupStatus_AfterInitialize_ShouldReturnConfigured()
    {
        await using var factory = new HiveLogsWebApplicationFactory();
        var client = factory.CreateClient();

        await client.PostAsJsonAsync(
            "/setup/initialize",
            new InitializeSetupRequest(
                SetupPassword,
                "Acme",
                "Admin",
                "admin@acme.com",
                "StrongPass123"));

        var response = await client.GetAsync("/setup/status");
        var body = await response.Content.ReadFromJsonAsync<SetupStatusResponse>(JsonOptions);

        body!.Status.Should().Be(SetupStatus.Configured);
        body.SetupRequired.Should().BeFalse();
    }
}
