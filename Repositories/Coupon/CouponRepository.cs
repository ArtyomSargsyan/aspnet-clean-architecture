using Microsoft.EntityFrameworkCore;
using ToDoApi.Data; 
using ToDoApi.Domain;

namespace ToDoApi.Repositories;

public class CouponRepository : ICouponRepository
{
    private readonly AppDbContext _context;

    public CouponRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Coupon?> GetByCodeAsync(string code)
    {
        return await _context.Coupons
            .FirstOrDefaultAsync(c => EF.Functions.ILike(c.Code, code));
    }

    public async Task<Coupon> CreateAsync(Coupon coupon)
    {
        _context.Coupons.Add(coupon);
        await _context.SaveChangesAsync();
        return coupon;
    }
}