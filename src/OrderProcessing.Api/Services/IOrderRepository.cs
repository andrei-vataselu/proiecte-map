using OrderProcessing.Api.Domain;

namespace OrderProcessing.Api.Services;

public interface IOrderRepository
{
    void Save(Order order);
    Order? GetById(OrderId id);
    IReadOnlyList<Order> GetAll();
}
