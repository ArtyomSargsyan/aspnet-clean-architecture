using Microsoft.EntityFrameworkCore;
using ToDoApi.Data;
using ToDoApi.Domain;

namespace ToDoApi.Repositories;

public class CouponRepository(AppDbContext context) : ICouponRepository
{

    public async Task<Coupon?> GetByCodeAsync(string code)
    {
        return await context.Coupons
            .FirstOrDefaultAsync(c => EF.Functions.ILike(c.Code, code));
    }

    public async Task<IReadOnlyList<Coupon>> GetAllAsync(bool activeOnly = false)
    {
        var query = context.Coupons.AsQueryable();

        if (activeOnly)
            query = query.Where(c => c.ExpiresAt > DateTime.UtcNow);

        return await query
            .OrderBy(c => c.ExpiresAt)
            .ToListAsync();
    }

    public async Task<Coupon> CreateAsync(Coupon coupon)
    {
        context.Coupons.Add(coupon);
        await context.SaveChangesAsync();
        return coupon;
    }
}
