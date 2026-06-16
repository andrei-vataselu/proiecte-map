namespace OrderProcessing.Api.Services;

public record CustomerRequest(string Name, string Email, int Age, bool IsTrusted);

public record AddressRequest(string Street, string City, string PostalCode, string Country);

public record OrderItemRequest(
    Guid ProductId,
    string ProductName,
    int Quantity,
    decimal UnitPrice,
    bool HasAgeRestriction);

public record CreateOrderRequest(
    CustomerRequest Customer,
    AddressRequest Address,
    List<OrderItemRequest> Items,
    decimal Total);
