using OrderProcessing.Api.Domain;

namespace OrderProcessing.Api.Validation;

public class ValidationContext
{
    public Customer Customer { get; init; } = null!;
    public Address Address { get; init; } = null!;
    public IReadOnlyList<OrderItem> Items { get; init; } = null!;
    public decimal DeclaredTotal { get; init; }
}
