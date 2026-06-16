namespace OrderProcessing.Api.Validation;

public abstract class BaseValidationHandler : IOrderValidationHandler
{
    private IOrderValidationHandler? _next;

    public IOrderValidationHandler SetNext(IOrderValidationHandler next)
    {
        _next = next;
        return next;
    }

    public ValidationResult Handle(ValidationContext context)
    {
        var result = Validate(context);
        if (!result.IsValid)
            return result;

        return _next?.Handle(context) ?? ValidationResult.Ok();
    }

    protected abstract ValidationResult Validate(ValidationContext context);
}
