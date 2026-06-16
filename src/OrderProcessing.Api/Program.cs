using OrderProcessing.Api.Endpoints;
using OrderProcessing.Api.Services;
using OrderProcessing.Api.Validation;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseUrls("http://localhost:5000");

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IProductStockRepository, InMemoryProductStockRepository>();
builder.Services.AddSingleton<IOrderRepository, InMemoryOrderRepository>();
builder.Services.AddSingleton<IOrderValidationHandler>(sp =>
{
    var stock = sp.GetRequiredService<IProductStockRepository>();
    var chain = new StockValidationHandler(stock);
    chain.SetNext(new PriceValidationHandler())
        .SetNext(new FraudDetectionHandler())
        .SetNext(new AgeVerificationHandler());
    return chain;
});
builder.Services.AddSingleton<OrderService>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseDefaultFiles();
app.UseStaticFiles();

app.MapOrderEndpoints();
app.MapFallbackToFile("index.html");

app.Run();
