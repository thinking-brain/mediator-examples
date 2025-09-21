# Postman Collection for Mediator Examples

This directory contains a comprehensive Postman collection for testing both **MediatR** and **MassTransit** example APIs.

## Files

- `Mediator-Examples-Collection.postman_collection.json` - The main Postman collection
- `Mediator-Examples.postman_environment.json` - Environment variables for the collection

## Setup Instructions

1. **Import Collection**:
   - Open Postman
   - Click "Import" 
   - Select `Mediator-Examples-Collection.postman_collection.json`

2. **Import Environment**:
   - Click "Import" again
   - Select `Mediator-Examples.postman_environment.json`
   - Set this as your active environment

3. **Start the APIs**:
   ```bash
   # Terminal 1 - MediatR Example
   cd MeaditR.Example
   dotnet run
   
   # Terminal 2 - MassTransit Example  
   cd MassTransit.Example
   dotnet run
   ```

## Available Endpoints

### MediatR.Example API (Port 7076/5092)

Traditional in-process mediator pattern using MediatR:

1. **Place Order - Success** ✅
2. **Place Order - Single Product** ✅  
3. **Place Order - High Value Order** ✅
4. **Place Order - Invalid Customer** ❌
5. **Place Order - Invalid Product** ❌
6. **Place Order - Empty Lines** ❌
7. **Place Order - Invalid JSON** ❌

### MassTransit.Example API (Port 7200/5155)

Message-driven architecture using MassTransit:

1. **Place Order - Success** ✅
   - Request/Response pattern with message consumers
2. **Place Order - Invalid Customer** ❌
   - Tests message-based validation
3. **Get Products - All** ✅
   - Query pattern with MassTransit consumers  
4. **Get Products - Specific IDs** ✅
   - Parameterized queries via messaging
5. **Check Customer Exists - Valid** ✅
   - Simple request/response for existence check
6. **Check Customer Exists - Invalid** ❌
   - Tests non-existent customer flow

## Architecture Comparison

| Feature | MediatR Example | MassTransit Example |
|---------|----------------|-------------------|
| **Communication** | In-process calls | Message-based |
| **Scalability** | Single service | Distributed |
| **Fault Tolerance** | Basic try/catch | Built-in retry/dead letter |
| **Event Handling** | Synchronous notifications | Asynchronous message consumers |
| **Transport** | Memory | Configurable (In-Memory, RabbitMQ, etc.) |

## Sample Data

Both APIs use the same sample data:

### Customers
- **John Doe**: `11111111-1111-1111-1111-111111111111`
- **Jane Smith**: `22222222-2222-2222-2222-222222222222`

### Products
- **Laptop**: `aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa` ($999.99)
- **Mouse**: `bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb` ($29.99)
- **Keyboard**: `cccccccc-cccc-cccc-cccc-cccccccccccc` ($79.99)

## Environment Variables

| Variable | MediatR Value | MassTransit Value | Description |
|----------|---------------|-------------------|-------------|
| `baseUrl` | `https://localhost:7076` | N/A | MediatR HTTPS |
| `httpBaseUrl` | `http://localhost:5092` | N/A | MediatR HTTP |
| `massTransitBaseUrl` | N/A | `https://localhost:7200` | MassTransit HTTPS |
| `massTransitHttpBaseUrl` | N/A | `http://localhost:5155` | MassTransit HTTP |

## Expected Behaviors

### MediatR Example Console Output
```
info: MeaditR.Example.Services.EmailService[0]
      Sending email to customer 11111111-1111-1111-1111-111111111111: Your order f47ac10b-58cc-4372-a567-0e02b2c3d479 was placed!
info: MeaditR.Example.Services.LoyaltyService[0]
      Adding 105 loyalty points to customer 11111111-1111-1111-1111-111111111111
```

### MassTransit Example Console Output
```
info: MassTransit[0]
      Configured endpoint PlaceOrderCommand, Consumer: MassTransit.Example.Orders.PlaceOrderCommandConsumer
info: MassTransit.Example.Orders.SendConfirmationEmailConsumer[0]
      Sending confirmation email for order f47ac10b-58cc-4372-a567-0e02b2c3d479 to customer 11111111-1111-1111-1111-111111111111
```

## Testing Message Flows

### Place Order Flow Comparison

**MediatR Flow:**
1. HTTP Request → Controller
2. Send Command → CommandHandler
3. Publish Event → Multiple NotificationHandlers
4. Return Response

**MassTransit Flow:**
1. HTTP Request → API Endpoint
2. Send Message → PlaceOrderCommandConsumer
3. Publish Event → Multiple Event Consumers (parallel)
4. Return Response via Message

### Event Handler Execution

Both examples trigger the same business logic:
- ✅ Send confirmation email
- ✅ Add loyalty points  
- ✅ Publish integration event
- ✅ Audit logging

**Key Difference:** MediatR executes handlers synchronously, MassTransit executes consumers asynchronously.

## Use Case Recommendations

### Choose MediatR When:
- Building monolithic applications
- Need simple in-process communication
- Want minimal overhead
- CQRS within single service

### Choose MassTransit When:
- Building distributed systems
- Need message durability
- Planning horizontal scaling
- Require fault tolerance
- Building microservices

## Troubleshooting

### Port Conflicts
Update `launchSettings.json` in each project if ports are already in use.

### SSL Issues
Use the HTTP endpoints if SSL certificate issues occur.

### MassTransit Consumer Issues
Check the console output for consumer configuration messages - all consumers should be listed on startup.