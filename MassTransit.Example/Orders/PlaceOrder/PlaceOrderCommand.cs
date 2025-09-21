namespace MassTransit.Example.Orders.PlaceOrder;

// Command to place an order (Request/Response pattern)
public record PlaceOrderCommand
{
    public Guid CustomerId { get; init; }
    public List<OrderLineItem> Lines { get; init; } = new();
}

public record OrderLineItem
{
    public Guid ProductId { get; init; }
    public int Quantity { get; init; }
    public decimal UnitPrice { get; init; }
}

// Response for place order command
public record PlaceOrderResult
{
    public Guid OrderId { get; init; }
    public bool Success { get; init; }
    public string? ErrorMessage { get; init; }
}

// Event published when order is created
public record OrderCreatedEvent
{
    public Guid OrderId { get; init; }
    public Guid CustomerId { get; init; }
    public decimal TotalAmount { get; init; }
    public DateTime CreatedAt { get; init; }
    public List<OrderLineItem> Lines { get; init; } = new();
}