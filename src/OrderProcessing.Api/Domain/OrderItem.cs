namespace OrderProcessing.Api.Domain;

public record OrderItem(
    Guid ProductId,
    string ProductName,
    int Quantity,
    Money UnitPrice,
    bool HasAgeRestriction);
