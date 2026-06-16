# OrderProcessing - Laborator 9 (MAP)

Sistem de procesare comenzi cu **Chain of Responsibility** (validare la creare) si **State** (ciclul de viata al comenzii). Backend ASP.NET Core Minimal API + SPA vanilla JavaScript.

## Cerinte indeplinite


| Pattern                 | Implementare                                                                            |
| ----------------------- | --------------------------------------------------------------------------------------- |
| Chain of Responsibility | `IOrderValidationHandler` + 4 handler-i (Stock -> Price -> Fraud -> Age), short-circuit |
| State                   | `IOrderState` + 6 stari, tranzitii decentralizate, `InvalidOrderTransitionException`    |


## Rulare

```bash
dotnet build OrderProcessing.sln
dotnet run --project src/OrderProcessing.Api/OrderProcessing.Api.csproj
```

- **SPA**: [http://localhost:5000](http://localhost:5000)
- **Swagger**: [http://localhost:5000/swagger](http://localhost:5000/swagger)

Teste HTTP: `src/OrderProcessing.Api/docs/requests.http`

## Structura

```
OrderProcessing.sln
└── src/OrderProcessing.Api/
    ├── Domain/          - Order, Customer, value objects
    ├── States/          - State Pattern (6 stari)
    ├── Validation/      - Chain of Responsibility
    ├── Services/        - OrderService, repository in-memory
    ├── Endpoints/       - Minimal API
    ├── wwwroot/         - SPA (index.html, styles.css, app.js)
    └── docs/            - requests.http, screenshots/
```

## Order Lifecycle

```mermaid
stateDiagram-v2
    [*] --> Pending
    Pending --> Confirmed: Pay()
    Pending --> Cancelled: Cancel()
    Confirmed --> Processing: Process()
    Confirmed --> Cancelled: Cancel()
    Processing --> Shipped: Ship()
    Processing --> Cancelled: Cancel()
    Shipped --> Delivered: Deliver()
    Delivered --> [*]
    Cancelled --> [*]
```



## Diagrama de clase

```mermaid
classDiagram
    class Order {
        +Status
        +Pay()
        +Process()
        +Ship()
        +Deliver()
        +Cancel()
    }
    class IOrderState {
        <<interface>>
        +Name
        +Pay()
        +Process()
        +Ship()
        +Deliver()
        +Cancel()
    }
    class PendingState
    class ConfirmedState
    class ProcessingState
    class ShippedState
    class DeliveredState
    class CancelledState
    class IOrderValidationHandler {
        <<interface>>
        +SetNext()
        +Handle()
    }
    class BaseValidationHandler
    class StockValidationHandler
    class PriceValidationHandler
    class FraudDetectionHandler
    class AgeVerificationHandler
    class OrderService

    Order --> IOrderState
    IOrderState <|.. PendingState
    IOrderState <|.. ConfirmedState
    IOrderState <|.. ProcessingState
    IOrderState <|.. ShippedState
    IOrderState <|.. DeliveredState
    IOrderState <|.. CancelledState
    IOrderValidationHandler <|.. BaseValidationHandler
    BaseValidationHandler <|-- StockValidationHandler
    BaseValidationHandler <|-- PriceValidationHandler
    BaseValidationHandler <|-- FraudDetectionHandler
    BaseValidationHandler <|-- AgeVerificationHandler
    OrderService --> IOrderValidationHandler
    OrderService --> Order
```



## Secventa: creare + livrare

```mermaid
sequenceDiagram
    participant SPA
    participant API
    participant Svc as OrderService
    participant Chain as Validation Chain
    participant Order

    SPA->>API: POST /orders
    API->>Svc: CreateOrder()
    Svc->>Chain: Handle(context)
    Chain-->>Svc: ValidationResult OK
    Svc->>Order: new Order (PendingState)
    Svc-->>API: Order
    API-->>SPA: 201 Created

    SPA->>API: POST /orders/{id}/pay
    API->>Svc: PayOrder()
    Svc->>Order: Pay() -> ConfirmedState
    API-->>SPA: 200 Order

    SPA->>API: POST /orders/{id}/process
    API->>Svc: ProcessOrder()
    Svc->>Order: Process() -> ProcessingState

    SPA->>API: POST /orders/{id}/ship
    API->>Svc: ShipOrder()
    Svc->>Order: Ship() -> ShippedState

    SPA->>API: POST /orders/{id}/deliver
    API->>Svc: DeliverOrder()
    Svc->>Order: Deliver() -> DeliveredState
    API-->>SPA: 200 Order
```



## Screenshot-uri

Adauga capturi in `src/OrderProcessing.Api/docs/screenshots/`:

1. Comanda nou creata (Pending, Pay + Cancel active)
2. Comanda Shipped (doar Deliver enabled)
3. Eroare tranzitie invalida (toast rosu, 409)
4. Validare esuata la creare (stoc / varsta)

## Endpoint-uri


| Metoda | Ruta                   | Descriere                        |
| ------ | ---------------------- | -------------------------------- |
| POST   | `/orders`              | Creeaza comanda (chain validare) |
| GET    | `/orders`              | Lista comenzi                    |
| GET    | `/orders/{id}`         | Detalii + history                |
| POST   | `/orders/{id}/pay`     | Pending -> Confirmed             |
| POST   | `/orders/{id}/process` | Confirmed -> Processing          |
| POST   | `/orders/{id}/ship`    | Processing -> Shipped            |
| POST   | `/orders/{id}/deliver` | Shipped -> Delivered             |
| POST   | `/orders/{id}/cancel`  | -> Cancelled (daca permis)       |


