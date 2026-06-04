namespace HiveLogs.Api.Infrastructure.Cors;

public static class CorsExtensions
{
    public const string WebDashboardPolicy = "WebDashboard";

    public static IServiceCollection AddHiveLogsCors(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var origins = configuration.GetSection("Cors:AllowedOrigins").Get<string[]>()
            ??
            [
                "http://localhost:5173",
                "http://127.0.0.1:5173",
                "http://localhost:5174",
                "http://127.0.0.1:5174",
            ];

        services.AddCors(options =>
        {
            options.AddPolicy(WebDashboardPolicy, policy =>
            {
                policy.WithOrigins(origins)
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            });
        });

        return services;
    }

    public static IApplicationBuilder UseHiveLogsCors(this IApplicationBuilder app) =>
        app.UseCors(WebDashboardPolicy);
}
