using ToDoApi.Domain;

namespace ToDoApi.Repositories;

public interface ICouponRepository
{
    Task<Coupon?> GetByCodeAsync(string code);
    Task<IReadOnlyList<Coupon>> GetAllAsync(bool activeOnly = false);
    Task<Coupon> CreateAsync(Coupon coupon);
}
