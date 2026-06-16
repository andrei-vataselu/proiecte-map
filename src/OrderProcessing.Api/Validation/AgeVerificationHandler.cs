namespace OrderProcessing.Api.Validation;

public class AgeVerificationHandler : BaseValidationHandler
{
    protected override ValidationResult Validate(ValidationContext context)
    {
        if (context.Items.Any(i => i.HasAgeRestriction) && context.Customer.Age < 18)
            return ValidationResult.Failed(
                "Comanda contine produse cu restrictie de varsta; clientul trebuie sa aiba minim 18 ani.");

        return ValidationResult.Ok();
    }
}
