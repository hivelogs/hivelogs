using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using HiveLogs.Api.Tests.Support;
using HiveLogs.Application.Applications.Requests;
using HiveLogs.Application.Applications.Responses;
using HiveLogs.Application.Environments.Requests;
using HiveLogs.Application.Environments.Responses;
using HiveLogs.Application.Organizations.Requests;
using HiveLogs.Application.Organizations.Responses;

namespace HiveLogs.Api.Tests;

public sealed class EnvironmentsEndpointTests(HiveLogsWebApplicationFactory factory)
    : IClassFixture<HiveLogsWebApplicationFactory>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task PostEnvironment_WithProductionName_ShouldReturn201AndNormalizeToLowercase()
    {
        var (organizationId, applicationId) = await SeedApplicationAsync();

        var response = await _client.PostAsJsonAsync(
            $"/organizations/{organizationId}/applications/{applicationId}/environments",
            new CreateEnvironmentRequest("PRODUCTION"));

        response.StatusCode.Should().Be(HttpStatusCode.Created);
        response.Headers.Location.Should().NotBeNull();

        var body = await response.Content.ReadFromJsonAsync<EnvironmentResponse>();
        body.Should().NotBeNull();
        body!.Name.Should().Be("production");
    }

    [Fact]
    public async Task PostEnvironment_WhenApplicationMissing_ShouldReturn404()
    {
        var organizationId = await CreateOrganizationAsync();

        var response = await _client.PostAsJsonAsync(
            $"/organizations/{organizationId}/applications/{Guid.NewGuid()}/environments",
            new CreateEnvironmentRequest("production"));

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);

        var problem = await ProblemDetailsResponse.ReadAsync(response);
        problem.Should().NotBeNull();
        problem!.Code.Should().Be("environments.application_not_found");
        problem.TraceId.Should().NotBeNullOrWhiteSpace();
    }

    [Fact]
    public async Task PostEnvironment_WithDuplicateName_ShouldReturn409()
    {
        var (organizationId, applicationId) = await SeedApplicationAsync();
        var path = $"/organizations/{organizationId}/applications/{applicationId}/environments";

        await _client.PostAsJsonAsync(path, new CreateEnvironmentRequest("production"));

        var response = await _client.PostAsJsonAsync(path, new CreateEnvironmentRequest("production"));

        response.StatusCode.Should().Be(HttpStatusCode.Conflict);

        var problem = await ProblemDetailsResponse.ReadAsync(response);
        problem.Should().NotBeNull();
        problem!.Code.Should().Be("environments.name_already_exists");
        problem.TraceId.Should().NotBeNullOrWhiteSpace();
    }

    [Theory]
    [InlineData("")]
    [InlineData("Invalid Name!")]
    public async Task PostEnvironment_WithInvalidName_ShouldReturn400(string name)
    {
        var (organizationId, applicationId) = await SeedApplicationAsync();

        var response = await _client.PostAsJsonAsync(
            $"/organizations/{organizationId}/applications/{applicationId}/environments",
            new CreateEnvironmentRequest(name));

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var problem = await ProblemDetailsResponse.ReadAsync(response);
        problem.Should().NotBeNull();
        problem!.Code.Should().BeOneOf("environments.name_required", "environments.name_invalid");
        problem.TraceId.Should().NotBeNullOrWhiteSpace();
    }

    [Fact]
    public async Task GetEnvironmentById_WhenMissing_ShouldReturn404()
    {
        var (organizationId, applicationId) = await SeedApplicationAsync();

        var response = await _client.GetAsync(
            $"/organizations/{organizationId}/applications/{applicationId}/environments/{Guid.NewGuid()}");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);

        var problem = await ProblemDetailsResponse.ReadAsync(response);
        problem.Should().NotBeNull();
        problem!.Code.Should().Be("environments.not_found");
        problem.TraceId.Should().NotBeNullOrWhiteSpace();
    }

    [Fact]
    public async Task GetEnvironments_WhenApplicationMissing_ShouldReturn404()
    {
        var organizationId = await CreateOrganizationAsync();

        var response = await _client.GetAsync(
            $"/organizations/{organizationId}/applications/{Guid.NewGuid()}/environments");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);

        var problem = await ProblemDetailsResponse.ReadAsync(response);
        problem.Should().NotBeNull();
        problem!.Code.Should().Be("environments.application_not_found");
    }

    private async Task<(Guid OrganizationId, Guid ApplicationId)> SeedApplicationAsync()
    {
        var organizationId = await CreateOrganizationAsync($"Org-{Guid.NewGuid():N}");

        var appResponse = await _client.PostAsJsonAsync(
            $"/organizations/{organizationId}/applications",
            new CreateApplicationRequest("Web API"));

        appResponse.EnsureSuccessStatusCode();
        var app = await appResponse.Content.ReadFromJsonAsync<ApplicationResponse>();

        return (organizationId, app!.Id);
    }

    private async Task<Guid> CreateOrganizationAsync(string? name = null)
    {
        var response = await _client.PostAsJsonAsync(
            "/organizations",
            new CreateOrganizationRequest(name ?? $"Org-{Guid.NewGuid():N}"));

        response.EnsureSuccessStatusCode();
        var body = await response.Content.ReadFromJsonAsync<OrganizationResponse>();
        return body!.Id;
    }
}
