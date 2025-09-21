# Postman Collection for Mediator Examples

This directory contains a comprehensive Postman collection for testing the Mediator Examples APIs.

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

3. **Start the API**:
   ```bash
   cd MeaditR.Example
   dotnet run
   ```

## Available Endpoints

### MediatR.Example API

The collection includes comprehensive tests for the **Place Order** endpoint:

#### Test Scenarios

1. **Place Order - Success** ✅
   - Valid customer and products
   - Tests successful order placement
   - Demonstrates MediatR command handling

2. **Place Order - Single Product** ✅
   - Simple order with one item
   - Uses Jane Smith's customer ID

3. **Place Order - High Value Order** ✅
   - Multiple expensive items
   - Tests loyalty points calculation

4. **Place Order - Invalid Customer** ❌
   - Non-existent customer ID
   - Tests business rule validation

5. **Place Order - Invalid Product** ❌
   - Non-existent product ID
   - Tests product validation

6. **Place Order - Empty Lines** ❌
   - Order with no items
   - Tests edge case handling

7. **Place Order - Invalid JSON** ❌
   - Invalid data types
   - Tests model binding validation

## Sample Data

The collection uses the following pre-configured test data:

### Customers
- **John Doe**: `11111111-1111-1111-1111-111111111111`
- **Jane Smith**: `22222222-2222-2222-2222-222222222222`

### Products
- **Laptop**: `aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa` ($999.99)
- **Mouse**: `bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb` ($29.99)
- **Keyboard**: `cccccccc-cccc-cccc-cccc-cccccccccccc` ($79.99)

## Environment Variables

The environment includes the following variables:

| Variable | Value | Description |
|----------|-------|-------------|
| `baseUrl` | `https://localhost:7076` | HTTPS endpoint |
| `httpBaseUrl` | `http://localhost:5092` | HTTP endpoint |
| `validCustomerId1` | John Doe's ID | Valid customer |
| `validCustomerId2` | Jane Smith's ID | Valid customer |
| `laptopProductId` | Laptop ID | Product ID |
| `mouseProductId` | Mouse ID | Product ID |
| `keyboardProductId` | Keyboard ID | Product ID |
| `invalidCustomerId` | Invalid ID | For error testing |
| `invalidProductId` | Invalid ID | For error testing |

## Expected Behaviors

When you run the requests, you should see:

### Console Logs (API Side)
The application will log the following events:
- Email confirmation sent
- Loyalty points added
- Integration events published
- Audit logs recorded

### Response Examples

**Success Response:**
```json
{
    "orderId": "f47ac10b-58cc-4372-a567-0e02b2c3d479",
    "message": "Order placed successfully"
}
```

**Error Response:**
```json
{
    "error": "Invalid customer"
}
```

## Testing the MediatR Pattern

This collection demonstrates several key MediatR concepts:

1. **Command Handling**: `PlaceOrderCommand` → `PlaceOrderHandler`
2. **Domain Events**: `OrderCreated` notification
3. **Multiple Handlers**: Email, Loyalty, Integration, Audit
4. **Dependency Injection**: All services injected via DI
5. **Error Handling**: Business rule validation

## Next Steps

You can extend this collection by adding:
- Query endpoints (when implemented)
- Additional MediatR examples from other projects
- Pre-request scripts for dynamic data
- Test scripts for assertions
- Mock server responses

## Troubleshooting

### SSL Certificate Issues
If you encounter SSL issues with HTTPS, either:
1. Use the HTTP endpoint (`httpBaseUrl`)
2. Disable SSL verification in Postman settings
3. Add the development certificate to your trusted store

### Port Conflicts
If the default ports are in use, update the `launchSettings.json` and environment variables accordingly.