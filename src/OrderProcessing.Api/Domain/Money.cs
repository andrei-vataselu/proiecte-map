namespace OrderProcessing.Api.Domain;

public record Money(decimal Amount, string Currency = "RON")
{
    public static Money FromItems(IEnumerable<OrderItem> items) =>
        new(items.Sum(i => i.Quantity * i.UnitPrice.Amount));
}
