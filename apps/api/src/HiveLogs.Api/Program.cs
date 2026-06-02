using HiveLogs.Api.Infrastructure.ExceptionHandling;
using HiveLogs.IoC;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHiveLogsDependencies(builder.Configuration);
builder.Services.AddControllers();
builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

var app = builder.Build();

app.UseExceptionHandler();
app.MapControllers();

app.MapGet("/health", () => Results.Ok(new
{
    status = "healthy",
    service = "hivelogs-api"
}));

app.Run();

public partial class Program;
