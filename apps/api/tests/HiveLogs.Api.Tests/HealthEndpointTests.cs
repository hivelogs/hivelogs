using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;

namespace HiveLogs.Api.Tests;

public class HealthEndpointTests(HiveLogsWebApplicationFactory factory)
    : IClassFixture<HiveLogsWebApplicationFactory>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task GetHealth_ShouldReturn200()
    {
        var response = await _client.GetAsync("/health");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task GetHealth_ShouldReturnHealthyStatus()
    {
        var response = await _client.GetAsync("/health");
        var body = await response.Content.ReadFromJsonAsync<HealthResponse>();

        body.Should().NotBeNull();
        body!.Status.Should().Be("healthy");
        body.Service.Should().Be("hivelogs-api");
    }

    private sealed record HealthResponse(string Status, string Service);
}
