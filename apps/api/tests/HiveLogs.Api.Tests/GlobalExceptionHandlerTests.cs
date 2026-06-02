using FluentAssertions;
using HiveLogs.Api.Infrastructure.ExceptionHandling;
using HiveLogs.Domain.Common.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging.Abstractions;

namespace HiveLogs.Api.Tests;

public class GlobalExceptionHandlerTests
{
    [Fact]
    public async Task TryHandleAsync_UnhandledException_ShouldReturnProblemDetails500()
    {
        var handler = CreateHandler(Environments.Production);
        var httpContext = new DefaultHttpContext();
        httpContext.Response.Body = new MemoryStream();
        httpContext.TraceIdentifier = "test-trace-id";

        var handled = await handler.TryHandleAsync(
            httpContext,
            new InvalidOperationException("Sensitive internal detail"),
            CancellationToken.None);

        handled.Should().BeTrue();
        httpContext.Response.StatusCode.Should().Be(500);
        httpContext.Response.Body.Position = 0;

        using var reader = new StreamReader(httpContext.Response.Body);
        var body = await reader.ReadToEndAsync();

        body.Should().Contain("general.unexpected");
        body.Should().Contain("test-trace-id");
        body.Should().NotContain("Sensitive internal detail");
    }

    [Fact]
    public void MapException_DomainException_ShouldReturn400()
    {
        var handler = CreateHandler(Environments.Production);

        var mapped = handler.MapException(new DomainException("Invariant violated"));

        mapped.StatusCode.Should().Be(400);
        mapped.Code.Should().Be("general.validation");
        mapped.Detail.Should().Be("Invariant violated");
    }

    private static GlobalExceptionHandler CreateHandler(string environmentName)
    {
        var environment = new TestHostEnvironment { EnvironmentName = environmentName };
        return new GlobalExceptionHandler(environment, NullLogger<GlobalExceptionHandler>.Instance);
    }

    private sealed class TestHostEnvironment : IHostEnvironment
    {
        public string EnvironmentName { get; set; } = Environments.Production;
        public string ApplicationName { get; set; } = "HiveLogs.Api.Tests";
        public string ContentRootPath { get; set; } = AppContext.BaseDirectory;
        public IFileProvider ContentRootFileProvider { get; set; } = new NullFileProvider();
    }
}
