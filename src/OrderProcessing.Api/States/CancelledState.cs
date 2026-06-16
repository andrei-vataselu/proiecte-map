using OrderProcessing.Api.Domain;

namespace OrderProcessing.Api.States;

public class CancelledState : IOrderState
{
    public string Name => NameConst;
    public const string NameConst = "Cancelled";

    public void Pay(Order order) => Throw("Pay");
    public void Process(Order order) => Throw("Process");
    public void Ship(Order order) => Throw("Ship");
    public void Deliver(Order order) => Throw("Deliver");
    public void Cancel(Order order) => Throw("Cancel");

    private static void Throw(string action) =>
        throw new InvalidOrderTransitionException(NameConst, action);
}
