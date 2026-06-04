using System.Text.Json.Serialization;
using HiveLogs.Api.Infrastructure.Cors;
using HiveLogs.Api.Infrastructure.ExceptionHandling;
using HiveLogs.IoC;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHiveLogsDependencies(builder.Configuration);
builder.Services.AddHiveLogsCors(builder.Configuration);
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });
builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

var app = builder.Build();

app.UseExceptionHandler();
app.UseHiveLogsCors();
app.MapControllers();

app.MapGet("/health", () => Results.Ok(new
{
    status = "healthy",
    service = "hivelogs-api"
}));

app.Run();

public partial class Program;
