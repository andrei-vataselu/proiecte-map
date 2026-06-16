namespace OrderProcessing.Api.Validation;

public class StockValidationHandler : BaseValidationHandler
{
    private readonly IProductStockRepository _stock;

    public StockValidationHandler(IProductStockRepository stock) => _stock = stock;

    protected override ValidationResult Validate(ValidationContext context)
    {
        foreach (var item in context.Items)
        {
            var available = _stock.GetStock(item.ProductId);
            if (available < item.Quantity)
                return ValidationResult.Failed(
                    $"Produsul '{item.ProductName}' nu este in stoc suficient (disponibil: {available}, cerut: {item.Quantity}).");
        }

        return ValidationResult.Ok();
    }
}
