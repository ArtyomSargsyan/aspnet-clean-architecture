using System;

namespace ToDoApi.Domain;

public class Coupon
{
    public int Id { get; private set; }
    public string Code { get; private set; } = string.Empty;
    public decimal DiscountPercentage { get; private set; }

    private Coupon() { }

    public Coupon(string code, decimal discountPercentage)
    {
        if (string.IsNullOrWhiteSpace(code))
            throw new ArgumentException("Coupon code cannot be empty.");

        if (discountPercentage <= 0 || discountPercentage > 1)
            throw new ArgumentException("Discount percentage must be between 0 and 1 (e.g., 0.15 for 15%).");

        Code = code;
        DiscountPercentage = discountPercentage;
    }
}