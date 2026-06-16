namespace OrderProcessing.Api.Validation;

public class PriceValidationHandler : BaseValidationHandler
{
    protected override ValidationResult Validate(ValidationContext context)
    {
        foreach (var item in context.Items)
        {
            if (item.UnitPrice.Amount <= 0)
                return ValidationResult.Failed($"Pretul pentru '{item.ProductName}' trebuie sa fie pozitiv.");

            if (item.Quantity <= 0)
                return ValidationResult.Failed($"Cantitatea pentru '{item.ProductName}' trebuie sa fie pozitiva.");
        }

        var computed = context.Items.Sum(i => i.Quantity * i.UnitPrice.Amount);
        if (computed != context.DeclaredTotal)
            return ValidationResult.Failed(
                $"Totalul declarat ({context.DeclaredTotal} RON) nu coincide cu suma calculata ({computed} RON).");

        return ValidationResult.Ok();
    }
}
