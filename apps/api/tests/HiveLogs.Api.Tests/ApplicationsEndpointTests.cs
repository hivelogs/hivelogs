using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using HiveLogs.Api.Tests.Support;
using HiveLogs.Application.Applications.Requests;
using HiveLogs.Application.Applications.Responses;
using HiveLogs.Application.Organizations.Requests;
using HiveLogs.Application.Organizations.Responses;

namespace HiveLogs.Api.Tests;

public sealed class ApplicationsEndpointTests(HiveLogsWebApplicationFactory factory)
    : IClassFixture<HiveLogsWebApplicationFactory>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task PostApplication_WithValidRequest_ShouldReturn201Created()
    {
        var organizationId = await CreateOrganizationAsync("Acme Corp");

        var response = await _client.PostAsJsonAsync(
            $"/organizations/{organizationId}/applications",
            new CreateApplicationRequest("Web API"));

        response.StatusCode.Should().Be(HttpStatusCode.Created);
        response.Headers.Location.Should().NotBeNull();

        var body = await response.Content.ReadFromJsonAsync<ApplicationResponse>();
        body.Should().NotBeNull();
        body!.Name.Should().Be("Web API");
        body.OrganizationId.Should().Be(organizationId);
    }

    [Fact]
    public async Task PostApplication_WhenOrganizationMissing_ShouldReturn404()
    {
        var response = await _client.PostAsJsonAsync(
            $"/organizations/{Guid.NewGuid()}/applications",
            new CreateApplicationRequest("Web API"));

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);

        var problem = await ProblemDetailsResponse.ReadAsync(response);
        problem.Should().NotBeNull();
        problem!.Code.Should().Be("applications.organization_not_found");
        problem.TraceId.Should().NotBeNullOrWhiteSpace();
    }

    [Theory]
    [InlineData("")]
    public async Task PostApplication_WithInvalidName_ShouldReturn400(string name)
    {
        var organizationId = await CreateOrganizationAsync();

        var response = await _client.PostAsJsonAsync(
            $"/organizations/{organizationId}/applications",
            new CreateApplicationRequest(name));

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var problem = await ProblemDetailsResponse.ReadAsync(response);
        problem.Should().NotBeNull();
        problem!.Code.Should().Be("applications.name_required");
        problem.TraceId.Should().NotBeNullOrWhiteSpace();
    }

    [Fact]
    public async Task PostApplication_WithDuplicateName_ShouldReturn409()
    {
        var organizationId = await CreateOrganizationAsync();

        await _client.PostAsJsonAsync(
            $"/organizations/{organizationId}/applications",
            new CreateApplicationRequest("Web API"));

        var response = await _client.PostAsJsonAsync(
            $"/organizations/{organizationId}/applications",
            new CreateApplicationRequest("web api"));

        response.StatusCode.Should().Be(HttpStatusCode.Conflict);

        var problem = await ProblemDetailsResponse.ReadAsync(response);
        problem.Should().NotBeNull();
        problem!.Code.Should().Be("applications.name_already_exists");
        problem.TraceId.Should().NotBeNullOrWhiteSpace();
    }

    [Fact]
    public async Task GetApplicationById_WhenMissing_ShouldReturn404()
    {
        var organizationId = await CreateOrganizationAsync();

        var response = await _client.GetAsync(
            $"/organizations/{organizationId}/applications/{Guid.NewGuid()}");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);

        var problem = await ProblemDetailsResponse.ReadAsync(response);
        problem.Should().NotBeNull();
        problem!.Code.Should().Be("applications.not_found");
        problem.TraceId.Should().NotBeNullOrWhiteSpace();
    }

    [Fact]
    public async Task GetApplications_WhenOrganizationMissing_ShouldReturn404()
    {
        var response = await _client.GetAsync($"/organizations/{Guid.NewGuid()}/applications");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);

        var problem = await ProblemDetailsResponse.ReadAsync(response);
        problem.Should().NotBeNull();
        problem!.Code.Should().Be("applications.organization_not_found");
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
