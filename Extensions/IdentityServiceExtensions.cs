using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace ToDoApi.Extensions;

public static class IdentityServiceExtensions
{
    // 'this IServiceCollection services' - սա կոչվում է Extension Method: 
    // Այն թույլ է տալիս builder.Services-ին ստանալ նոր ֆունկցիա:
    public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration config)
    {
        // 1. Ասում ենք ASP.NET-ին, որ օգտագործելու ենք Authentication (նույնականացում)
        // և հիմնական սխեման լինելու է JwtBearer:
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                // 2. Վերցնում ենք գաղտնի բանալին (Secret Key) քո appsettings.json-ից
                var keyString = config["Jwt:Key"];

                // Սա Senior-ական մոտեցում է՝ եթե բանալին չկա, ծրագիրը թող հենց սկզբից կանգնի (Fail Fast)
                if (string.IsNullOrEmpty(keyString))
                    throw new InvalidOperationException("JWT Key is missing in configuration!");

                var key = Encoding.UTF8.GetBytes(keyString);

                // 3. Token Validation Parameters - Սա այն «ֆիլտրն» է, որով ստուգվում է ամեն եկող հարցում
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,   // Ստուգել՝ արդյոք մեր սերվերն է թողարկել այս token-ը
                    ValidateAudience = true, // Ստուգել՝ արդյոք token-ը նախատեսված է հենց մեր API-ի համար
                    ValidateLifetime = true, // Ստուգել՝ արդյոք token-ի ժամկետը չի անցել
                    ValidateIssuerSigningKey = true, // Ստուգել բանալու իսկությունը

                    ValidIssuer = config["Jwt:Issuer"],
                    ValidAudience = config["Jwt:Audience"],

                    // Սա ամենակարևոր մասն է՝ ստորագրությունը, որը փակում է token-ը
                    IssuerSigningKey = new SymmetricSecurityKey(key),

                    // Ասում ենք, որ Role-երը փնտրի "Role" claim-ի մեջ (Laravel-ի պես)
                    RoleClaimType = "Role",

                    // Կարևոր դետալ՝ հանում ենք լռելյայն 5 րոպեանոց «հանդուրժողականությունը»
                    ClockSkew = TimeSpan.Zero
                };
            });

        return services;
    }
}