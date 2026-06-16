using OrderProcessing.Api.States;

namespace OrderProcessing.Api.Domain;

public class Order
{
    internal IOrderState CurrentState { get; set; } = new PendingState();

    public OrderId Id { get; }
    public Customer Customer { get; }
    public Address ShippingAddress { get; }
    public IReadOnlyList<OrderItem> Items { get; }
    public Money Total { get; }
    public string Status => CurrentState.Name;
    public List<OrderTransition> History { get; } = new();

    public Order(OrderId id, Customer customer, Address address, IEnumerable<OrderItem> items)
    {
        Id = id;
        Customer = customer;
        ShippingAddress = address;
        Items = items.ToList();
        Total = Money.FromItems(Items);
        History.Add(new OrderTransition("-", PendingState.NameConst, DateTime.UtcNow));
    }

    public void Pay() => CurrentState.Pay(this);
    public void Process() => CurrentState.Process(this);
    public void Ship() => CurrentState.Ship(this);
    public void Deliver() => CurrentState.Deliver(this);
    public void Cancel() => CurrentState.Cancel(this);

    internal void TransitionTo(IOrderState newState)
    {
        var from = CurrentState.Name;
        CurrentState = newState;
        History.Add(new OrderTransition(from, newState.Name, DateTime.UtcNow));
    }
}
