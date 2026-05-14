using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ToDoApi.Models;

namespace ToDoApi.Services.Auth;

public sealed class JwtService(IConfiguration config, TimeProvider? timeProvider = null)
{
    private const int MinKeyBytes = 32;

    private readonly string       _key         = ValidateKey(config["Jwt:Key"]);
    private readonly string       _issuer      = config["Jwt:Issuer"]   ?? string.Empty;
    private readonly string       _audience    = config["Jwt:Audience"] ?? string.Empty;
    private readonly int          _expiryHours = int.TryParse(config["Jwt:ExpireHours"], out var h) ? h : 2;
    private readonly TimeProvider _time        = timeProvider ?? TimeProvider.System;

    public string GenerateToken(User user)
    {
        // C# 12 collection expression instead of `new[]`
        Claim[] claims =
        [
            new(JwtRegisteredClaimNames.Sub,        user.Id.ToString()),
            new(JwtRegisteredClaimNames.UniqueName, user.UserName),
            new(JwtRegisteredClaimNames.Email,      user.Email),
            new("Role",                             user.Role ?? "User"),
        ];

        var signingKey  = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_key));
        var credentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

        // TimeProvider.GetUtcNow() returns DateTimeOffset — no implicit UTC drift.
        var now = _time.GetUtcNow().UtcDateTime;

        var token = new JwtSecurityToken(
            issuer:             _issuer,
            audience:           _audience,
            claims:             claims,
            expires:            now.AddHours(_expiryHours),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    // Static helper: can be called from a field initializer (no `this` needed).
    private static string ValidateKey(string? key)
    {
        if (string.IsNullOrWhiteSpace(key))
            throw new InvalidOperationException(
                "JWT key is missing or empty. Provide 'Jwt:Key' via environment variables or user secrets — never in appsettings.json.");

        if (Encoding.UTF8.GetByteCount(key) < MinKeyBytes)
            throw new InvalidOperationException(
                $"JWT key is too short. HMAC-SHA256 requires at least {MinKeyBytes} bytes ({MinKeyBytes * 8} bits).");

        return key;
    }
}
