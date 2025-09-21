# Mediator Examples - MediatR vs MassTransit

This repository demonstrates two different approaches to implementing the **Mediator pattern** in .NET 9:

1. **MediatR.Example** - In-process mediator using MediatR library
2. **MassTransit.Example** - Message-driven architecture using MassTransit

Both examples implement the same e-commerce order processing domain to showcase the differences between the approaches.

## 🏗️ Project Structure

```
mediator-examples/
├── MeaditR.Example/              # In-process mediator with MediatR
│   ├── Orders/
│   │   ├── PlaceOrder/           # Order commands and handlers
│   │   └── OrderCreated/         # Order event handlers
│   ├── Catalogue/
│   │   └── GetProducts/          # Product queries and handlers
│   ├── Customer/
│   │   └── GetCustomerExist/     # Customer queries and handlers
│   └── README.md                 # MediatR-specific documentation
├── MassTransit.Example/          # Message-driven with MassTransit
│   ├── Orders/
│   │   ├── PlaceOrder/           # Order message consumers
│   │   └── OrderCreated/         # Order event consumers
│   ├── Catalogue/
│   │   └── GetProducts/          # Product query consumers
│   ├── Customer/
│   │   └── GetCustomerExist/     # Customer query consumers
│   └── README.md                 # MassTransit-specific documentation
├── Mediator.Core/                # Shared domain models and services
│   ├── Entities/                 # Domain entities (Order, OrderLine)
│   ├── Dtos/                     # Data transfer objects
│   ├── Repositories/             # Repository interfaces
│   └── Services/                 # Service interfaces and implementations
└── Postman/                      # API testing collection
    ├── Mediator-Examples-Collection.postman_collection.json
    ├── Mediator-Examples.postman_environment.json
    └── POSTMAN_README.md
```

## 🚀 Getting Started

### Prerequisites
- .NET 9 SDK
- Visual Studio Code or Visual Studio
- Postman (optional, for API testing)

### Running the Applications

1. **Clone the repository**:
   ```bash
   git clone <repository-url>
   cd mediator-examples
   ```

2. **Run MediatR Example**:
   ```bash
   cd MeaditR.Example
   dotnet run
   # API available at: https://localhost:7076
   ```

3. **Run MassTransit Example** (in separate terminal):
   ```bash
   cd MassTransit.Example  
   dotnet run
   # API available at: https://localhost:7200
   ```

### Testing with Postman

Import the provided Postman collection and environment:
- `Mediator-Examples-Collection.postman_collection.json`
- `Mediator-Examples.postman_environment.json`

See [POSTMAN_README.md](POSTMAN_README.md) for detailed instructions.

## 📋 Implemented Features

Both examples implement the same business functionality:

### Core Features
- ✅ **Place Order** - Create orders with validation
- ✅ **Get Products** - Retrieve product information  
- ✅ **Check Customer** - Validate customer existence
- ✅ **Event Processing** - Handle order created events

### Event Handlers
When an order is placed, both systems trigger:
- 📧 **Email Confirmation** - Send order confirmation
- 🎯 **Loyalty Points** - Award points based on order total
- 📤 **Integration Events** - Notify external systems
- 📝 **Audit Logging** - Record order creation

### Sample Data
- **Customers**: John Doe, Jane Smith
- **Products**: Laptop ($999.99), Mouse ($29.99), Keyboard ($79.99)

## 🔍 Code Examples

### MediatR Command Handler
```csharp
public class PlaceOrderHandler : IRequestHandler<PlaceOrderCommand, Guid>
{
    public async Task<Guid> Handle(PlaceOrderCommand request, CancellationToken ct)
    {
        // Validate and create order
        var order = Order.Create(request.CustomerId, request.Lines, products);
        await _orders.AddAsync(order, ct);

        // Publish domain event (synchronous)
        await _mediator.Publish(new OrderCreated(order.Id, order.CustomerId, order.TotalAmount));
        
        return order.Id;
    }
}
```

### MassTransit Message Consumer
```csharp
public class PlaceOrderCommandConsumer : IConsumer<PlaceOrderCommand>
{
    public async Task Consume(ConsumeContext<PlaceOrderCommand> context)
    {
        // Validate and create order
        var order = CreateOrder(context.Message, products);
        await _orderRepository.AddAsync(order);

        // Publish domain event (asynchronous)
        await _publishEndpoint.Publish(new OrderCreatedEvent { ... });
        
        // Respond with result
        await context.RespondAsync(new PlaceOrderResult { OrderId = order.Id });
    }
}
```

## 🧪 Testing Scenarios

Both implementations provide comprehensive test scenarios:

### Success Cases
- Simple order placement
- Multi-item orders
- High-value orders (loyalty testing)

### Error Cases  
- Invalid customer ID
- Invalid product ID
- Empty order lines
- Malformed requests

## 📚 Learning Outcomes

By exploring both examples, you'll learn:

1. **Pattern Implementation**: How to implement the mediator pattern using different approaches
2. **Architectural Trade-offs**: Understanding when to use each approach
3. **Message-Driven Design**: How MassTransit enables distributed architectures
4. **Event-Driven Architecture**: Handling domain events in both patterns
5. **Testing Strategies**: How to test both in-process and message-based systems

## 🔧 Production Considerations

### MediatR in Production
- Add behaviors for cross-cutting concerns (validation, logging, caching)
- Implement proper exception handling strategies
- Consider using FluentValidation for request validation

### MassTransit in Production
- Choose appropriate transport (RabbitMQ, Azure Service Bus, Amazon SQS)
- Implement proper error handling and retry policies
- Configure monitoring and health checks
- Set up proper message serialization
- Consider implementing sagas for complex workflows

## 🤝 Contributing

Feel free to submit issues, fork the repository, and create pull requests for any improvements.

## 📄 License

This project is licensed under the GNU General Public License - see the LICENSE file for details.
