using Microsoft.EntityFrameworkCore;
using ToDoApi.Data;
using ToDoApi.Extensions;
using ToDoApi.Middleware;

var builder = WebApplication.CreateBuilder(args);

// 1. SERVICES CONFIGURATION (Dependency Injection)

builder.Services.AddControllers()
    .AddJsonOptions(opt => {
        opt.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
        opt.JsonSerializerOptions.WriteIndented = true;
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Database
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Extension
builder.Services.AddApplicationServices();
builder.Services.AddIdentityServices(builder.Configuration); // JWT

var app = builder.Build();

// 2. MIDDLEWARE PIPELINE 
app.UseErrorHandling();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My Project API V1");
        c.RoutePrefix = string.Empty;
    });
}

// Routing -> Auth -> Controllers
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();  
app.UseAdminRole();      

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await DbSeeder.SeedAsync(db);
}

app.Run();