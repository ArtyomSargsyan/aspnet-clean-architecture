using FluentValidation;
using ToDoApi.Features.Orders.Commands;

namespace ToDoApi.Features.Orders.Validators;

public class CreateOrderValidator : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderValidator()
    {
        RuleFor(x => x.ProductId)
            .GreaterThan(0)
            .WithMessage("Product ID must be valid.");

        RuleFor(x => x.Quantity)
            .GreaterThan(0)
            .WithMessage("Quantity must be greater than zero.");

        RuleFor(x => x.OriginalPrice)
            .GreaterThan(0)
            .WithMessage("Price must be positive.");
    }
}