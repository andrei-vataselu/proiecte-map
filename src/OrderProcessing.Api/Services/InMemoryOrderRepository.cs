using OrderProcessing.Api.Domain;

namespace OrderProcessing.Api.Services;

public class InMemoryOrderRepository : IOrderRepository
{
    private readonly Dictionary<Guid, Order> _orders = new();

    public void Save(Order order) => _orders[order.Id.Value] = order;

    public Order? GetById(OrderId id) =>
        _orders.TryGetValue(id.Value, out var order) ? order : null;

    public IReadOnlyList<Order> GetAll() =>
        _orders.Values.OrderByDescending(o => o.History.Last().At).ToList();
}
