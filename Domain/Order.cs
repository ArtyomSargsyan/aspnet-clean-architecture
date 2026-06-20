namespace ToDoApi.Domain;

public class Order
{
    public int Id { get; private set; }
    public int ProductId { get; private set; }
    public int Quantity { get; private set; }

    public decimal SubTotal { get; private set; }
    public decimal FinalPrice { get; private set; }
    public string? AppliedCouponCode { get; private set; }
    public DateTime CreatedAt { get; private set; }

    private Order() { }

    public Order(int productId, int quantity, decimal originalPrice)
    {
        if (quantity <= 0) throw new ArgumentException("Quantity must be greater then zero");
        if (originalPrice <= 0) throw new ArgumentException("Price must be positive.");

        ProductId = productId;
        Quantity = quantity;
        SubTotal = originalPrice * quantity;
        FinalPrice = SubTotal;
        CreatedAt = DateTime.UtcNow;
    }

    public void ApplyCoupon(Coupon coupon)
    {
        if (coupon.DiscountPercentage < 0 || coupon.DiscountPercentage > 1)
            throw new ArgumentException("Invalid discount percentage.");

        AppliedCouponCode = coupon.Code;
        decimal discountAmount = SubTotal * coupon.DiscountPercentage;
        FinalPrice = SubTotal - discountAmount;
    }
}
