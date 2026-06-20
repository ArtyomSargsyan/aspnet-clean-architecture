# aspnet-clean-architecture

A RESTful Web API built with **ASP.NET Core 9** and **PostgreSQL**, intentionally demonstrating two modern API architectural styles side by side.

---

## Tech Stack

| Layer | Technology |
|-------|-----------|
| Framework | ASP.NET Core 9 (.NET 9) |
| Database | PostgreSQL via EF Core 9 + Npgsql |
| Auth | JWT Bearer Authentication |
| Validation | FluentValidation / DataAnnotations |
| Mediator | MediatR 14 |
| Password Hashing | BCrypt.Net |
| Seeding | Bogus (fake data) |
| Docs | Swagger / OpenAPI |

---

## Architecture

This project deliberately uses **two API styles** to demonstrate both approaches:

### Style 1 — MVC Controllers (Traditional)
Used by: `Products`, `Articles`, `Categories`, `Auth`

```
Controller → Service → Repository → DbContext
```

- Controllers handle HTTP routing and response shaping
- Services contain business logic
- DataAnnotations for request validation
- Repository pattern for data access abstraction

### Style 2 — Minimal API + CQRS (Modern)
Used by: `Orders`, `Coupons`

```
Endpoint → MediatR Handler → Repository → DbContext
```

- Minimal API endpoints registered via `IEndpoint` + reflection
- MediatR decouples request from handler
- FluentValidation via MediatR pipeline behavior
- Commands/Queries separated by intent

---

## Project Structure

```
ToDoApi/
├── Controllers/          # MVC Controllers (Style 1)
├── Endpoints/            # Minimal API Endpoints (Style 2)
├── Features/             # CQRS — Commands, Handlers, Validators (Style 2)
│   └── Orders/
├── Services/             # Business logic layer (Style 1)
├── Repositories/         # Data access layer
├── Domain/               # Domain entities with encapsulation (Order, Coupon)
├── Models/               # EF Core entities (Product, Category, User…)
├── DTO/                  # Data Transfer Objects
├── Exceptions/           # Custom exception hierarchy
├── Extensions/           # IServiceCollection & IApplicationBuilder extensions
├── Behaviors/            # MediatR pipeline behaviors (ValidationBehavior)
├── Infrastructure/       # Cross-cutting concerns (Storage, etc.)
├── Middleware/           # Custom middleware
├── Data/                 # DbContext + Seeder
└── Migrations/           # EF Core migrations
```

---

## Key Patterns

- **Repository Pattern** — abstracts data access behind interfaces (`IProductRepository`, `ICouponRepository`)
- **CQRS with MediatR** — Orders use Commands/Handlers, decoupled from HTTP layer
- **Validation Pipeline** — `ValidationBehavior<TRequest, TResponse>` runs FluentValidation before every MediatR handler
- **Global Exception Handling** — custom `AppException` hierarchy (`NotFoundException`, `ConflictException`, `ForbiddenException`) caught by middleware
- **IEndpoint abstraction** — all Minimal API endpoints implement `IEndpoint` and are auto-registered via reflection
- **Domain encapsulation** — `Order` and `Coupon` use private setters, constructor validation, and business methods (`ApplyCoupon`)

---

## Getting Started

### Prerequisites
- .NET 9 SDK
- PostgreSQL

### Configuration

Create a `.env` file or set `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=todoapi;Username=postgres;Password=yourpassword"
  },
  "Jwt": {
    "Key": "your-secret-key",
    "Issuer": "ToDoApi",
    "Audience": "ToDoApi"
  }
}
```

### Run

```bash
dotnet ef database update
dotnet run
```

Swagger UI opens at `http://localhost:<port>/`

---

## API Overview

| Area | Style | Auth | Route |
|------|-------|------|-------|
| Auth | MVC | Public | `POST /api/auth/login` |
| Articles | MVC | Public | `GET/POST/DELETE /api/article` |
| Products | MVC | Admin | `GET/POST/PUT/DELETE /api/admin/products` |
| Categories | MVC | Public | `/api/category` |
| Orders | Minimal API | Public | `POST /api/orders` |
| Coupons | Minimal API | Public | `GET/POST /api/coupons` |

---

## Running Tests

```bash
dotnet test
```
