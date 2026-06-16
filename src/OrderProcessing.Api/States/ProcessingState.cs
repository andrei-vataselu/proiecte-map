using OrderProcessing.Api.Domain;

namespace OrderProcessing.Api.States;

public class ProcessingState : IOrderState
{
    public string Name => ProcessingState.NameConst;
    public const string NameConst = "Processing";

    public void Ship(Order order) => order.TransitionTo(new ShippedState());
    public void Cancel(Order order) => order.TransitionTo(new CancelledState());
    public void Pay(Order order) => throw new InvalidOrderTransitionException(Name, "Pay");
    public void Process(Order order) => throw new InvalidOrderTransitionException(Name, "Process");
    public void Deliver(Order order) => throw new InvalidOrderTransitionException(Name, "Deliver");
}
