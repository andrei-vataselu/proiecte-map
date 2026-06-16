using OrderProcessing.Api.Services;
using OrderProcessing.Api.States;
using OrderProcessing.Api.Validation;

namespace OrderProcessing.Api.Endpoints;

public static class OrderEndpoints
{
    public static void MapOrderEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/orders");

        group.MapPost("/", (CreateOrderRequest request, OrderService service) =>
        {
            var outcome = service.CreateOrder(request);
            if (outcome.ValidationError is { } err)
                return Results.BadRequest(new { errors = err.Errors });

            return Results.Created($"/orders/{outcome.Order!.Id}", outcome.Order);
        });

        group.MapGet("/", (OrderService service) => Results.Ok(service.GetAllOrders()));

        group.MapGet("/{id:guid}", (Guid id, OrderService service) =>
        {
            var order = service.GetOrder(new Domain.OrderId(id));
            return order is null ? Results.NotFound() : Results.Ok(order);
        });

        group.MapPost("/{id:guid}/pay", (Guid id, OrderService service) =>
            Transition(id, service.PayOrder));

        group.MapPost("/{id:guid}/process", (Guid id, OrderService service) =>
            Transition(id, service.ProcessOrder));

        group.MapPost("/{id:guid}/ship", (Guid id, OrderService service) =>
            Transition(id, service.ShipOrder));

        group.MapPost("/{id:guid}/deliver", (Guid id, OrderService service) =>
            Transition(id, service.DeliverOrder));

        group.MapPost("/{id:guid}/cancel", (Guid id, OrderService service) =>
            Transition(id, service.CancelOrder));

        app.MapGet("/products", (IProductStockRepository stock) =>
        {
            var products = new[]
            {
                new { Id = InMemoryProductStockRepository.LaptopId, Name = "Laptop Pro", DefaultPrice = 4500m, HasAgeRestriction = false, Stock = stock.GetStock(InMemoryProductStockRepository.LaptopId) },
                new { Id = InMemoryProductStockRepository.WineId, Name = "Vin rosu premium", DefaultPrice = 89m, HasAgeRestriction = true, Stock = stock.GetStock(InMemoryProductStockRepository.WineId) },
                new { Id = InMemoryProductStockRepository.MouseId, Name = "Mouse wireless", DefaultPrice = 120m, HasAgeRestriction = false, Stock = stock.GetStock(InMemoryProductStockRepository.MouseId) },
                new { Id = InMemoryProductStockRepository.GpuId, Name = "Placa video RTX", DefaultPrice = 5200m, HasAgeRestriction = false, Stock = stock.GetStock(InMemoryProductStockRepository.GpuId) }
            };
            return Results.Ok(products);
        });
    }

    private static IResult Transition(Guid id, Func<Domain.OrderId, Domain.Order> action)
    {
        try
        {
            return Results.Ok(action(new Domain.OrderId(id)));
        }
        catch (KeyNotFoundException)
        {
            return Results.NotFound();
        }
        catch (InvalidOrderTransitionException ex)
        {
            return Results.Conflict(new { message = ex.Message });
        }
    }
}
