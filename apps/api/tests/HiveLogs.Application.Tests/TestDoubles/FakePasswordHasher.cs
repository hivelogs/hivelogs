using HiveLogs.Application.Abstractions.Security;

namespace HiveLogs.Application.Tests.TestDoubles;

internal sealed class FakePasswordHasher : IPasswordHasher
{
    public string Hash(string password) => $"hashed:{password}";

    public bool Verify(string passwordHash, string password) =>
        passwordHash == Hash(password);
}
