# MassTransit Example Project

This project demonstrates **message-based architecture** using **MassTransit** in .NET 9. It showcases an e-commerce order processing system using message consumers, request/response patterns, and event publishing.

## Architecture Overview

The project implements a **message-driven architecture** where:
- **Commands** are processed via request/response patterns
- **Events** are published and consumed by multiple handlers
- **Queries** use request/response for data retrieval
- All communication happens through MassTransit's messaging infrastructure

### Key Differences from MediatR Example

| Aspect | MediatR Example | MassTransit Example |
|--------|----------------|-------------------|
| **Communication** | In-process mediator | Message-based (can be distributed) |
| **Handlers** | Direct method calls | Message consumers |
| **Events** | Synchronous notifications | Asynchronous message publishing |
| **Scalability** | Single process | Can scale across multiple services |
| **Fault Tolerance** | Basic try/catch | Built-in retry, error handling |

## Project Structure

```
MassTransit.Example/
├── Orders/
│   ├── PlaceOrder/
│   │   ├── PlaceOrderCommand.cs      # Command and response definitions
│   │   └── PlaceOrderHandler.cs      # Command consumer
│   └── OrderCreated/
│       └── NotificationHandlers.cs   # Event consumers
├── Catalogue/
│   └── GetProducts/
│       ├── GetProductsQuery.cs       # Query and response definitions
│       └── GetProductsHandler.cs     # Query consumer
├── Customer/
│   └── GetCustomerExist/
│       ├── GetCustomerExistQuery.cs  # Query and response definitions
│       └── GetCustomerExistHandler.cs # Query consumer
├── Program.cs                        # MassTransit configuration and endpoints
└── MassTransit.Example.http         # API testing examples
```

## Message Definitions

### Commands (Request/Response)
- `PlaceOrderCommand` → `PlaceOrderResult`
- `GetProductsQuery` → `GetProductsResponse`  
- `GetCustomerExistQuery` → `GetCustomerExistResponse`

### Events (Publish/Subscribe)
- `OrderCreatedEvent` → Multiple consumers handle this event

## Implemented Features

### 1. Place Order (Request/Response)
**Endpoint**: `POST /place-order`

**Flow**:
1. API receives HTTP request
2. Sends `PlaceOrderCommand` message to consumer
3. `PlaceOrderCommandConsumer` processes the command
4. Validates business rules (customer exists, products exist)
5. Creates and persists order
6. Publishes `OrderCreatedEvent`
7. Returns `PlaceOrderResult` to API
8. API responds to HTTP client

### 2. Order Created Event Processing
When `OrderCreatedEvent` is published, four consumers process it **concurrently**:

1. **`SendConfirmationEmailConsumer`**: Sends email confirmation
2. **`AddLoyaltyPointsConsumer`**: Adds loyalty points  
3. **`PublishIntegrationEventConsumer`**: Publishes to external systems
4. **`AuditLogConsumer`**: Logs for audit trail

### 3. Get Products (Request/Response)
**Endpoint**: `GET /products?productIds=guid1,guid2`

Demonstrates query patterns with MassTransit request/response.

### 4. Check Customer Existence (Request/Response)
**Endpoint**: `GET /customers/{customerId}/exists`

Simple query to check if customer exists.

## MassTransit Configuration

The project uses **In-Memory transport** for demonstration:

```csharp
builder.Services.AddMassTransit(x =>
{
    // Register all consumers
    x.AddConsumer<PlaceOrderCommandConsumer>();
    x.AddConsumer<SendConfirmationEmailConsumer>();
    // ... other consumers

    // Use in-memory transport
    x.UsingInMemory((context, cfg) =>
    {
        cfg.ConfigureEndpoints(context);
    });
});
```

## Sample Data

Same sample data as MediatR example:

### Customers
- **John Doe**: `11111111-1111-1111-1111-111111111111`
- **Jane Smith**: `22222222-2222-2222-2222-222222222222`

### Products
- **Laptop**: `aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa` ($999.99)
- **Mouse**: `bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb` ($29.99)
- **Keyboard**: `cccccccc-cccc-cccc-cccc-cccccccccccc` ($79.99)

## Running the Application

1. **Start the application**:
   ```bash
   dotnet run --project MassTransit.Example/MassTransit.Example.csproj
   ```

2. **Test the API** using the HTTP file or curl:
   ```bash
   curl -X POST https://localhost:7001/place-order \
     -H "Content-Type: application/json" \
     -d '{
       "customerId": "11111111-1111-1111-1111-111111111111",
       "lines": [
         {
           "productId": "aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa",
           "quantity": 1,
           "unitPrice": 999.99
         }
       ]
     }'
   ```

## Key Benefits of MassTransit Architecture

1. **Distributed Processing**: Consumers can run in separate services
2. **Fault Tolerance**: Built-in retry, error handling, poison message handling
3. **Scalability**: Horizontal scaling by adding more consumer instances
4. **Observability**: Built-in metrics, health checks, and monitoring
5. **Transport Agnostic**: Can switch between In-Memory, RabbitMQ, Azure Service Bus, etc.
6. **Message Patterns**: Support for various patterns (Request/Response, Publish/Subscribe, etc.)

## MassTransit Features Demonstrated

- ✅ **Request/Response** patterns with `IRequestClient<T>`
- ✅ **Publish/Subscribe** with multiple consumers per event
- ✅ **Message consumers** with `IConsumer<T>`
- ✅ **Dependency injection** integration
- ✅ **In-Memory transport** for development/testing
- ✅ **Automatic endpoint configuration**
- ✅ **Error handling** with proper response patterns

## Production Considerations

For production use, consider:

### Transport Options
- **RabbitMQ**: Most popular, feature-rich
- **Azure Service Bus**: Cloud-native option for Azure
- **Amazon SQS**: AWS-native messaging
- **SQL Server**: Database-backed transport

### Configuration Example (RabbitMQ)
```csharp
x.UsingRabbitMq((context, cfg) =>
{
    cfg.Host("rabbitmq://localhost");
    cfg.ConfigureEndpoints(context);
});
```

### Advanced Features
- **Sagas** for long-running processes
- **Scheduling** for delayed/recurring messages
- **Outbox pattern** for reliable message publishing
- **Circuit breakers** for fault tolerance
- **Message encryption** for security

## Comparison: When to Use Which?

### Use **MediatR** when:
- Building monolithic applications
- Need simple in-process communication
- Want minimal overhead and complexity
- CQRS within a single service

### Use **MassTransit** when:
- Building distributed systems
- Need fault tolerance and reliability
- Planning to scale horizontally
- Require message durability and delivery guarantees
- Building microservices architecture

Both patterns can coexist - use MediatR within services and MassTransit between services!