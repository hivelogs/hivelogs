using System.Net.Http.Json;
using System.Text.Json;

namespace HiveLogs.Api.Tests.Support;

internal sealed class ProblemDetailsResponse
{
    public string? Type { get; init; }
    public string? Title { get; init; }
    public int? Status { get; init; }
    public string? Detail { get; init; }
    public string? Code { get; init; }
    public string? TraceId { get; init; }

    public static async Task<ProblemDetailsResponse?> ReadAsync(HttpResponseMessage response)
    {
        var document = await response.Content.ReadFromJsonAsync<JsonDocument>();
        if (document is null)
            return null;

        var root = document.RootElement;
        var extensions = root.TryGetProperty("extensions", out var ext) ? ext : default;

        return new ProblemDetailsResponse
        {
            Type = root.GetPropertyOrNull("type"),
            Title = root.GetPropertyOrNull("title"),
            Status = root.TryGetProperty("status", out var status) ? status.GetInt32() : null,
            Detail = root.GetPropertyOrNull("detail"),
            Code = extensions.ValueKind != JsonValueKind.Undefined
                ? extensions.GetPropertyOrNull("code")
                : root.GetPropertyOrNull("code"),
            TraceId = extensions.ValueKind != JsonValueKind.Undefined
                ? extensions.GetPropertyOrNull("traceId")
                : root.GetPropertyOrNull("traceId")
        };
    }
}

internal static class JsonElementExtensions
{
    public static string? GetPropertyOrNull(this JsonElement element, string propertyName) =>
        element.TryGetProperty(propertyName, out var value) ? value.GetString() : null;
}
