using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using MediatR;
using ToDoApi.Features.Orders.Commands;

namespace ToDoApi.Endpoints;

public class OrderEndpoints : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/orders")
                       .WithTags("Orders");

        group.MapPost("/", async (CreateOrderCommand command, IMediator mediator) =>
        {
            try
            {
                var createdOrder = await mediator.Send(command);
                return Results.Created($"/api/orders/{createdOrder.Id}", createdOrder);
            }
            catch (KeyNotFoundException ex)
            {
                return Results.NotFound(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return Results.BadRequest(ex.Message);
            }
        })
        .Produces(StatusCodes.Status201Created)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status404NotFound);
    }
}