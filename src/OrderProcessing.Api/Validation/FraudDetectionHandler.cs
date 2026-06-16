namespace OrderProcessing.Api.Validation;

public class FraudDetectionHandler : BaseValidationHandler
{
    private const decimal HighValueThreshold = 10_000m;

    protected override ValidationResult Validate(ValidationContext context)
    {
        if (!context.Customer.IsTrusted && context.DeclaredTotal > HighValueThreshold)
            return ValidationResult.Failed(
                $"Comanda depaseste {HighValueThreshold} RON si clientul nu este marcat ca trusted.");

        if (context.Items.Count > 50)
            return ValidationResult.Failed("Comanda contine mai mult de 50 de produse distincte.");

        return ValidationResult.Ok();
    }
}
