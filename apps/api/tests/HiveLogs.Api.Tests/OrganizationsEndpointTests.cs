using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using HiveLogs.Api.Tests.Support;
using HiveLogs.Application.Organizations.Requests;
using HiveLogs.Application.Organizations.Responses;

namespace HiveLogs.Api.Tests;

public sealed class OrganizationsEndpointTests(HiveLogsWebApplicationFactory factory)
    : IClassFixture<HiveLogsWebApplicationFactory>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task PostOrganization_WithValidRequest_ShouldReturn201Created()
    {
        var name = $"Acme-{Guid.NewGuid():N}";
        var response = await _client.PostAsJsonAsync(
            "/organizations",
            new CreateOrganizationRequest(name));

        response.StatusCode.Should().Be(HttpStatusCode.Created);
        response.Headers.Location.Should().NotBeNull();

        var body = await response.Content.ReadFromJsonAsync<OrganizationResponse>();
        body.Should().NotBeNull();
        body!.Name.Should().Be(name);
        body.Id.Should().NotBeEmpty();
    }

    [Theory]
    [InlineData("")]
    [InlineData("A")]
    public async Task PostOrganization_WithInvalidName_ShouldReturn400(string name)
    {
        var response = await _client.PostAsJsonAsync(
            "/organizations",
            new CreateOrganizationRequest(name));

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var problem = await ProblemDetailsResponse.ReadAsync(response);
        problem.Should().NotBeNull();
        problem!.Code.Should().Be("organizations.name_required");
        problem.TraceId.Should().NotBeNullOrWhiteSpace();
    }

    [Fact]
    public async Task PostOrganization_WithDuplicateName_ShouldReturn409()
    {
        await _client.PostAsJsonAsync("/organizations", new CreateOrganizationRequest("Acme Corp"));

        var response = await _client.PostAsJsonAsync(
            "/organizations",
            new CreateOrganizationRequest("acme corp"));

        response.StatusCode.Should().Be(HttpStatusCode.Conflict);

        var problem = await ProblemDetailsResponse.ReadAsync(response);
        problem.Should().NotBeNull();
        problem!.Code.Should().Be("organizations.name_already_exists");
        problem.TraceId.Should().NotBeNullOrWhiteSpace();
    }

    [Fact]
    public async Task GetOrganizationById_WhenMissing_ShouldReturn404()
    {
        var response = await _client.GetAsync($"/organizations/{Guid.NewGuid()}");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);

        var problem = await ProblemDetailsResponse.ReadAsync(response);
        problem.Should().NotBeNull();
        problem!.Code.Should().Be("organizations.not_found");
        problem.TraceId.Should().NotBeNullOrWhiteSpace();
    }

    [Fact]
    public async Task GetOrganizations_ShouldReturn200WithList()
    {
        var created = await _client.PostAsJsonAsync(
            "/organizations",
            new CreateOrganizationRequest($"Org-{Guid.NewGuid():N}"));

        var createdBody = await created.Content.ReadFromJsonAsync<OrganizationResponse>();

        var response = await _client.GetAsync("/organizations");

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var list = await response.Content.ReadFromJsonAsync<List<OrganizationResponse>>();
        list.Should().Contain(o => o.Id == createdBody!.Id);
    }

    [Fact]
    public async Task GetOrganizationById_WhenExists_ShouldReturn200()
    {
        const string name = "Acme Corp";
        var created = await _client.PostAsJsonAsync(
            "/organizations",
            new CreateOrganizationRequest(name));
        var createdBody = await created.Content.ReadFromJsonAsync<OrganizationResponse>();

        var response = await _client.GetAsync($"/organizations/{createdBody!.Id}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var body = await response.Content.ReadFromJsonAsync<OrganizationResponse>();
        body!.Id.Should().Be(createdBody.Id);
        body.Name.Should().Be(name);
    }
}
