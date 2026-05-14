using FluentValidation;
using MediatR;
using ToDoApi.Behaviors;
using ToDoApi.Infrastructure.Storage;
using ToDoApi.Repositories.Categories;
using ToDoApi.Repositories.Products;
using ToDoApi.Repositories.Users;
using ToDoApi.Services.Auth;
using ToDoApi.Services.Categories;
using ToDoApi.Services.Products;
using ToDoApi.Services.ProductModel;

namespace ToDoApi.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // ── Infrastructure ────────────────────────────────────────────────────

        // Real clock for production; tests inject FakeTimeProvider instead.
        services.AddSingleton(TimeProvider.System);

        // JwtService reads config once at startup — safe as a singleton.
        // Depends on IConfiguration (singleton) and TimeProvider (singleton above).
        services.AddSingleton<JwtService>();

        // ── Repositories ──────────────────────────────────────────────────────

        services.AddScoped<InterfaceProductRepository, ProductRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IUserRepository, UserRepository>();

        // ── Domain services ───────────────────────────────────────────────────

        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<IProductModelService, ProductModelService>();
        services.AddScoped<IEmailService, SmtpEmailService>();

        // ── Strategies / Factories / Managers ─────────────────────────────────

        services.AddScoped<EmailSendStrategy>();
        services.AddScoped<ISendStrategyFactory, SendStrategyFactory>();
        services.AddScoped<INotificationManager, NotificationManager>();

        // ── Storage ───────────────────────────────────────────────────────────

        services.AddScoped<IBookStore, InMemoryBookStore>();

        // ── Cross-cutting concerns ─────────────────────────────────────────────

        services.AddErrorHandling();

        services.AddValidatorsFromAssembly(typeof(Program).Assembly);

        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(Program).Assembly);

            // C# 12 collection expression for the open-generic behavior type args.
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        });

        return services;
    }
}
