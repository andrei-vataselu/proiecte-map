using OrderProcessing.Api.Domain;

namespace OrderProcessing.Api.States;

public class PendingState : IOrderState
{
    public string Name => PendingState.NameConst;
    public const string NameConst = "Pending";

    public void Pay(Order order) => order.TransitionTo(new ConfirmedState());
    public void Cancel(Order order) => order.TransitionTo(new CancelledState());
    public void Process(Order order) => throw new InvalidOrderTransitionException(Name, "Process");
    public void Ship(Order order) => throw new InvalidOrderTransitionException(Name, "Ship");
    public void Deliver(Order order) => throw new InvalidOrderTransitionException(Name, "Deliver");
}
