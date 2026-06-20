using Microsoft.EntityFrameworkCore;
using ToDoApi.Data;
using ToDoApi.Extensions;

var builder = WebApplication.CreateBuilder(args);

// 1. SERVICES CONFIGURATION (Dependency Injection)
builder.Services.AddControllers()
    .AddJsonOptions(opt =>
    {
        opt.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
        opt.JsonSerializerOptions.WriteIndented = true;
    });

// Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Database (PostgreSQL)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Custom Application & Identity Extensions
builder.Services.AddApplicationServices();
builder.Services.AddIdentityServices(builder.Configuration);

var app = builder.Build();

// 2. MIDDLEWARE PIPELINE 
app.UseErrorHandling();

// Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My Project API V1");
        c.RoutePrefix = string.Empty;
    });
}

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

// ENDPOINTS & CONTROLLERS MAPPING
app.MapAllEndpoints();
app.MapControllers();

// DATABASE SEEDING 
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await DbSeeder.SeedAsync(db);
}

app.Run();