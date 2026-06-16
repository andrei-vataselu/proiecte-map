namespace OrderProcessing.Api.Domain;

public record Customer(Guid Id, string Name, string Email, int Age, bool IsTrusted);
