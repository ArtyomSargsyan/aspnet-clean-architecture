using Microsoft.EntityFrameworkCore;
using ToDoApi.Data;

namespace ToDoApi.Extensions;

public static class MigrationExtensions
{
    public static async Task<bool> HandleMigrationArgsAsync(this WebApplication app, string[] args)
    {
        if (!args.Contains("--migrate"))
            return false;

        var logger = app.Services.GetRequiredService<ILogger<Program>>();

        try
        {
            await using var scope = app.Services.CreateAsyncScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            logger.LogInformation("Applying EF Core migrations...");
            await db.Database.MigrateAsync();

            logger.LogInformation("Seeding database...");
            await DbSeeder.SeedAsync(db);

            logger.LogInformation("Migration and seeding completed successfully.");
        }
        catch (Exception ex)
        {
            logger.LogCritical(ex, "Migration or seeding failed. Shutting down.");
            Environment.Exit(1);
        }

        return true;
    }
}
