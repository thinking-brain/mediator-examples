namespace Mediator.Core.Dtos;

public record OrderLineDto(Guid ProductId, int Quantity, decimal UnitPrice);

public record Product(Guid Id, string Name, decimal Price, int StockQuantity);

public record Customer(Guid Id, string Name, string Email);

public record OrderCreatedIntegrationEvent(Guid OrderId, Guid CustomerId, decimal TotalAmount);