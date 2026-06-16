namespace OrderProcessing.Api.Validation;

public record ValidationResult(bool IsValid, IReadOnlyList<string> Errors)
{
    public static ValidationResult Ok() => new(true, Array.Empty<string>());
    public static ValidationResult Failed(string error) => new(false, new[] { error });
}
