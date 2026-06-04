using HiveLogs.Application.Abstractions.Persistence;
using HiveLogs.Application.Abstractions.Security;
using HiveLogs.Application.Abstractions.Time;
using HiveLogs.Infrastructure.Persistence;
using HiveLogs.Infrastructure.Persistence.Repositories;
using HiveLogs.Infrastructure.Security;
using HiveLogs.Infrastructure.Time;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HiveLogs.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        if (configuration.GetValue<bool>("Testing:UseInMemoryDatabase"))
        {
            var databaseName = configuration["Testing:InMemoryDatabaseName"]
                ?? Guid.NewGuid().ToString();

            services.AddDbContext<HiveLogsDbContext>(options =>
                options.UseInMemoryDatabase(databaseName)
                    .AddInterceptors(
                        new NameLowerSynchronizationInterceptor(),
                        new EmailLowerSynchronizationInterceptor()));
        }
        else
        {
            var connectionString = configuration.GetConnectionString("Default")
                ?? throw new InvalidOperationException("Connection string 'Default' is not configured.");

            services.AddDbContext<HiveLogsDbContext>(options =>
                options.UseNpgsql(connectionString));
        }

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IOrganizationRepository, OrganizationRepository>();
        services.AddScoped<IApplicationRepository, ApplicationRepository>();
        services.AddScoped<IEnvironmentRepository, EnvironmentRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IOrganizationMemberRepository, OrganizationMemberRepository>();
        services.AddScoped<ISetupStateRepository, SetupStateRepository>();
        services.AddSingleton<IPasswordHasher, PasswordHasher>();
        services.AddSingleton<ISetupPasswordValidator, SetupPasswordValidator>();
        services.AddSingleton<IClock, SystemClock>();

        return services;
    }
}
