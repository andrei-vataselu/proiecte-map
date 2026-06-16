namespace OrderProcessing.Api.Validation;

public interface IProductStockRepository
{
    int GetStock(Guid productId);
}

public class InMemoryProductStockRepository : IProductStockRepository
{
    public static readonly Guid LaptopId = Guid.Parse("11111111-1111-1111-1111-111111111111");
    public static readonly Guid WineId = Guid.Parse("22222222-2222-2222-2222-222222222222");
    public static readonly Guid MouseId = Guid.Parse("33333333-3333-3333-3333-333333333333");
    public static readonly Guid GpuId = Guid.Parse("44444444-4444-4444-4444-444444444444");

    private readonly Dictionary<Guid, int> _stock = new()
    {
        [LaptopId] = 25,
        [WineId] = 100,
        [MouseId] = 0,
        [GpuId] = 5
    };

    public int GetStock(Guid productId) =>
        _stock.TryGetValue(productId, out var qty) ? qty : 0;
}
