using MediatR;
using ToDoApi.Domain;
using ToDoApi.Features.Orders.Commands;
using ToDoApi.Repositories;
using ToDoApi.Repositories.Products;

namespace ToDoApi.Features.Orders.Handlers;

public class CreateOrderHandler : IRequestHandler<CreateOrderCommand, Order>
{
    private readonly IOrderRepository _orderRepository;
    private readonly ICouponRepository _couponRepository;
    private readonly IProductRepository _productRepository;

    public CreateOrderHandler(
        IOrderRepository orderRepository,
        ICouponRepository couponRepository,
        IProductRepository productRepository)
    {
        _orderRepository = orderRepository;
        _couponRepository = couponRepository;
        _productRepository = productRepository;
    }

    public async Task<Order> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(request.ProductId);
        if (product is null)
            throw new KeyNotFoundException($"Product with ID {request.ProductId} was not found.");

        var order = new Order(request.ProductId, request.Quantity, request.OriginalPrice);

        if (!string.IsNullOrWhiteSpace(request.CouponCode))
        {
            var coupon = await _couponRepository.GetByCodeAsync(request.CouponCode);
            if (coupon == null)
                throw new ArgumentException("The provided coupon code does not exist.");

            order.ApplyCoupon(coupon);
        }

        await _orderRepository.AddAsync(order);
        await _orderRepository.SaveChangesAsync();

        return order;
    }
}
