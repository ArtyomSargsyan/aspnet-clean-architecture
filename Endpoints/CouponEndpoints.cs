using ToDoApi.Domain;
using ToDoApi.Repositories;

namespace ToDoApi.Endpoints;

public class CouponEndpoints : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/coupons")
                       .WithTags("Coupons");

        // GET /api/coupons?activeOnly=true
        group.MapGet("/", async (ICouponRepository repo, bool activeOnly = false) =>
        {
            var coupons = await repo.GetAllAsync(activeOnly);
            return Results.Ok(coupons);
        })
        .Produces<IReadOnlyList<Coupon>>();

        // GET /api/coupons/{code}
        group.MapGet("/{code}", async (string code, ICouponRepository repo) =>
        {
            var coupon = await repo.GetByCodeAsync(code);

            if (coupon is null)
                return Results.NotFound($"Coupon '{code}' not found.");

            if (coupon.IsExpired)
                return Results.Problem(
                    title: "Coupon expired",
                    detail: $"Coupon '{coupon.Code}' expired on {coupon.ExpiresAt:yyyy-MM-dd HH:mm} UTC.",
                    statusCode: StatusCodes.Status410Gone);

            return Results.Ok(coupon);
        })
        .Produces<Coupon>()
        .Produces(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status410Gone);

        // POST /api/coupons
        group.MapPost("/", async (CreateCouponRequest request, ICouponRepository repo) =>
        {
            var existing = await repo.GetByCodeAsync(request.Code);
            if (existing is not null)
                return Results.Conflict($"Coupon '{request.Code}' already exists.");

            try
            {
                var coupon = new Coupon(request.Code, request.DiscountPercentage, request.ExpiresAt);
                var created = await repo.CreateAsync(coupon);
                return Results.Created($"/api/coupons/{created.Code}", created);
            }
            catch (ArgumentException ex)
            {
                return Results.BadRequest(ex.Message);
            }
        })
        .Produces<Coupon>(StatusCodes.Status201Created)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status409Conflict);
    }
}

public record CreateCouponRequest(string Code, decimal DiscountPercentage, DateTime ExpiresAt);
