using OrderProcessing.Api.Domain;

namespace OrderProcessing.Api.States;

public class ShippedState : IOrderState
{
    public string Name => ShippedState.NameConst;
    public const string NameConst = "Shipped";

    public void Deliver(Order order) => order.TransitionTo(new DeliveredState());
    public void Cancel(Order order) => throw new InvalidOrderTransitionException(Name, "Cancel");
    public void Pay(Order order) => throw new InvalidOrderTransitionException(Name, "Pay");
    public void Process(Order order) => throw new InvalidOrderTransitionException(Name, "Process");
    public void Ship(Order order) => throw new InvalidOrderTransitionException(Name, "Ship");
}
