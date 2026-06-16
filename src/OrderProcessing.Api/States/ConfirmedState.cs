using OrderProcessing.Api.Domain;

namespace OrderProcessing.Api.States;

public class ConfirmedState : IOrderState
{
    public string Name => ConfirmedState.NameConst;
    public const string NameConst = "Confirmed";

    public void Process(Order order) => order.TransitionTo(new ProcessingState());
    public void Cancel(Order order) => order.TransitionTo(new CancelledState());
    public void Pay(Order order) => throw new InvalidOrderTransitionException(Name, "Pay");
    public void Ship(Order order) => throw new InvalidOrderTransitionException(Name, "Ship");
    public void Deliver(Order order) => throw new InvalidOrderTransitionException(Name, "Deliver");
}
