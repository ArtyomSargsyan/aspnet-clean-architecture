using ToDoApi.Domain;
using ToDoApi.Repositories;

namespace ToDoApi.Endpoints;

public class CouponEndpoints : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/coupons")
                       .WithTags("Coupons");

        // GET /api/coupons/{code}  — validate / fetch coupon by code
        group.MapGet("/{code}", async (string code, ICouponRepository repo) =>
        {
            var coupon = await repo.GetByCodeAsync(code);
            return coupon is null
                ? Results.NotFound($"Coupon '{code}' not found.")
                : Results.Ok(coupon);
        })
        .Produces<Coupon>()
        .Produces(StatusCodes.Status404NotFound);

        // POST /api/coupons  — create a new coupon
        group.MapPost("/", async (CreateCouponRequest request, ICouponRepository repo) =>
        {
            var existing = await repo.GetByCodeAsync(request.Code);
            if (existing is not null)
                return Results.Conflict($"Coupon '{request.Code}' already exists.");

            try
            {
                var coupon = new Coupon(request.Code, request.DiscountPercentage);
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

public record CreateCouponRequest(string Code, decimal DiscountPercentage);
