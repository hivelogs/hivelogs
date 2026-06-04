using HiveLogs.Application.Abstractions.Persistence;
using HiveLogs.Domain.Users;

namespace HiveLogs.Application.Tests.TestDoubles;

internal sealed class InMemoryUserRepository : IUserRepository
{
    private readonly List<User> _users = [];

    public Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default) =>
        Task.FromResult(_users.Any(u =>
            string.Equals(u.Email.Value, email, StringComparison.OrdinalIgnoreCase)));

    public Task AddAsync(User user, CancellationToken cancellationToken = default)
    {
        _users.Add(user);
        return Task.CompletedTask;
    }

    public IReadOnlyList<User> Users => _users;
}
