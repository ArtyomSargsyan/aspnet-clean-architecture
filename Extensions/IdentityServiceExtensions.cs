using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace ToDoApi.Extensions;

public static class IdentityServiceExtensions
{
    // Must match the constant in JwtService — single source of truth for the rule.
    private const int MinKeyBytes = 32;

    public static IServiceCollection AddIdentityServices(
        this IServiceCollection services,
        IConfiguration config)
    {
        // ── Guard Clauses ─────────────────────────────────────────────────────
        // Same validation logic as JwtService.ValidateKey so the app fails fast
        // at startup rather than at the first token generation attempt.

        var keyString = config["Jwt:Key"];

        if (string.IsNullOrWhiteSpace(keyString))
            throw new InvalidOperationException(
                "JWT key is missing or empty. " +
                "Provide 'Jwt:Key' via environment variables or user secrets — never in appsettings.json.");

        if (Encoding.UTF8.GetByteCount(keyString) < MinKeyBytes)
            throw new InvalidOperationException(
                $"JWT key is too short. " +
                $"HMAC-SHA256 requires at least {MinKeyBytes} bytes ({MinKeyBytes * 8} bits).");

        // ── JWT Bearer authentication ─────────────────────────────────────────
        // IssuerSigningKey, ValidIssuer and ValidAudience intentionally mirror
        // the values used in JwtService.GenerateToken for consistency.

        var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyString));

        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer           = true,
                    ValidateAudience         = true,
                    ValidateLifetime         = true,
                    ValidateIssuerSigningKey = true,

                    ValidIssuer      = config["Jwt:Issuer"],
                    ValidAudience    = config["Jwt:Audience"],
                    IssuerSigningKey = signingKey,

                    // Claims created in JwtService use "Role" — must match here.
                    RoleClaimType = "Role",

                    // No tolerance for expired tokens.
                    ClockSkew = TimeSpan.Zero
                };
            });

        return services;
    }
}
