namespace ToDoApi.Domain;

public class Coupon
{
    public int Id { get; private set; }
    public string Code { get; private set; } = string.Empty;
    public decimal DiscountPercentage { get; private set; }
    public DateTime ExpiresAt { get; private set; }

    public bool IsExpired => DateTime.UtcNow > ExpiresAt;

    private Coupon() { }

    public Coupon(string code, decimal discountPercentage, DateTime expiresAt)
    {
        if (string.IsNullOrWhiteSpace(code))
            throw new ArgumentException("Coupon code cannot be empty.");

        if (discountPercentage <= 0 || discountPercentage > 1)
            throw new ArgumentException("Discount percentage must be between 0 and 1 (e.g. 0.25 for 25%).");

        if (expiresAt.ToUniversalTime() <= DateTime.UtcNow)
            throw new ArgumentException("Expiration date must be in the future.");

        Code = code.ToUpperInvariant();
        DiscountPercentage = discountPercentage;
        ExpiresAt = expiresAt.ToUniversalTime();
    }
}
