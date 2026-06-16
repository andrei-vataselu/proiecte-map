namespace OrderProcessing.Api.Validation;

public interface IOrderValidationHandler
{
    IOrderValidationHandler SetNext(IOrderValidationHandler next);
    ValidationResult Handle(ValidationContext context);
}
