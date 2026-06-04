using HiveLogs.Application.Abstractions.Security;
using Microsoft.AspNetCore.Identity;

namespace HiveLogs.Infrastructure.Security;

internal sealed class PasswordHasher : IPasswordHasher
{
    private readonly PasswordHasher<PasswordUser> _hasher = new();

    public string Hash(string password) =>
        _hasher.HashPassword(null!, password);

    public bool Verify(string passwordHash, string password) =>
        _hasher.VerifyHashedPassword(null!, passwordHash, password)
        == PasswordVerificationResult.Success;
}
