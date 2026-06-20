using MediatR;
using ToDoApi.Domain;

namespace ToDoApi.Features.Orders.Commands;

public record CreateOrderCommand(
    int ProductId,
    int Quantity,
    decimal OriginalPrice,
    string? CouponCode) : IRequest<Order>;
