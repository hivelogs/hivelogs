using HiveLogs.Application.Abstractions.Persistence;
using HiveLogs.Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace HiveLogs.Infrastructure.Persistence.Repositories;

internal sealed class UserRepository : IUserRepository
{
    private readonly HiveLogsDbContext _dbContext;

    public UserRepository(HiveLogsDbContext dbContext) => _dbContext = dbContext;

    public Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        var normalized = email.Trim().ToLowerInvariant();
        return _dbContext.Users.AnyAsync(
            u => EF.Property<string>(u, "EmailLower") == normalized,
            cancellationToken);
    }

    public async Task AddAsync(User user, CancellationToken cancellationToken = default) =>
        await _dbContext.Users.AddAsync(user, cancellationToken);
}
