using OrderProcessing.Api.Domain;
using OrderProcessing.Api.Validation;

namespace OrderProcessing.Api.Services;

public class CreateOrderOutcome
{
    public Order? Order { get; init; }
    public ValidationResult? ValidationError { get; init; }

    public static CreateOrderOutcome Success(Order order) => new() { Order = order };
    public static CreateOrderOutcome Failed(ValidationResult result) => new() { ValidationError = result };
}

public class OrderService
{
    private readonly IOrderRepository _repository;
    private readonly IOrderValidationHandler _validationChain;

    public OrderService(IOrderRepository repository, IOrderValidationHandler validationChain)
    {
        _repository = repository;
        _validationChain = validationChain;
    }

    public CreateOrderOutcome CreateOrder(CreateOrderRequest request)
    {
        var items = request.Items.Select(i =>
            new OrderItem(i.ProductId, i.ProductName, i.Quantity, new Money(i.UnitPrice), i.HasAgeRestriction)).ToList();

        var context = new ValidationContext
        {
            Customer = new Customer(Guid.NewGuid(), request.Customer.Name, request.Customer.Email,
                request.Customer.Age, request.Customer.IsTrusted),
            Address = new Address(request.Address.Street, request.Address.City,
                request.Address.PostalCode, request.Address.Country),
            Items = items,
            DeclaredTotal = request.Total
        };

        var validation = _validationChain.Handle(context);
        if (!validation.IsValid)
            return CreateOrderOutcome.Failed(validation);

        var order = new Order(OrderId.New(), context.Customer, context.Address, items);
        _repository.Save(order);
        return CreateOrderOutcome.Success(order);
    }

    public Order? GetOrder(OrderId id) => _repository.GetById(id);
    public IReadOnlyList<Order> GetAllOrders() => _repository.GetAll();

    public Order PayOrder(OrderId id) => Transition(id, o => o.Pay());
    public Order ProcessOrder(OrderId id) => Transition(id, o => o.Process());
    public Order ShipOrder(OrderId id) => Transition(id, o => o.Ship());
    public Order DeliverOrder(OrderId id) => Transition(id, o => o.Deliver());
    public Order CancelOrder(OrderId id) => Transition(id, o => o.Cancel());

    private Order Transition(OrderId id, Action<Order> action)
    {
        var order = _repository.GetById(id) ?? throw new KeyNotFoundException($"Comanda {id} nu exista.");
        action(order);
        _repository.Save(order);
        return order;
    }
}
