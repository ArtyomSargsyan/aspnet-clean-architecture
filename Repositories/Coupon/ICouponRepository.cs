using ToDoApi.Domain;

namespace ToDoApi.Repositories;

public interface ICouponRepository
{
    Task<Coupon?> GetByCodeAsync(string code);
    Task<Coupon> CreateAsync(Coupon coupon);
}