using Mediator.Core.Services;
using Mediator.Core.Dtos;
using MassTransit.Example.Orders.PlaceOrder;

namespace MassTransit.Example.Orders.OrderCreated;

// Send confirmation email when order is created
public class SendConfirmationEmailConsumer : IConsumer<OrderCreatedEvent>
{
    private readonly IEmailService _emailService;
    private readonly ILogger<SendConfirmationEmailConsumer> _logger;

    public SendConfirmationEmailConsumer(IEmailService emailService, ILogger<SendConfirmationEmailConsumer> logger)
    {
        _emailService = emailService;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<OrderCreatedEvent> context)
    {
        var orderCreated = context.Message;
        
        _logger.LogInformation("Sending confirmation email for order {OrderId} to customer {CustomerId}", 
            orderCreated.OrderId, orderCreated.CustomerId);

        await _emailService.SendAsync(orderCreated.CustomerId, 
            $"Your order {orderCreated.OrderId} was placed successfully!");
    }
}

// Add loyalty points when order is created
public class AddLoyaltyPointsConsumer : IConsumer<OrderCreatedEvent>
{
    private readonly ILoyaltyService _loyaltyService;
    private readonly ILogger<AddLoyaltyPointsConsumer> _logger;

    public AddLoyaltyPointsConsumer(ILoyaltyService loyaltyService, ILogger<AddLoyaltyPointsConsumer> logger)
    {
        _loyaltyService = loyaltyService;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<OrderCreatedEvent> context)
    {
        var orderCreated = context.Message;
        var points = (int)(orderCreated.TotalAmount / 10); // 1 point per $10

        _logger.LogInformation("Adding {Points} loyalty points for customer {CustomerId} from order {OrderId}", 
            points, orderCreated.CustomerId, orderCreated.OrderId);

        await _loyaltyService.AddPointsAsync(orderCreated.CustomerId, points);
    }
}

// Publish integration event to external systems
public class PublishIntegrationEventConsumer : IConsumer<OrderCreatedEvent>
{
    private readonly IEventBus _eventBus;
    private readonly ILogger<PublishIntegrationEventConsumer> _logger;

    public PublishIntegrationEventConsumer(IEventBus eventBus, ILogger<PublishIntegrationEventConsumer> logger)
    {
        _eventBus = eventBus;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<OrderCreatedEvent> context)
    {
        var orderCreated = context.Message;

        _logger.LogInformation("Publishing integration event for order {OrderId}", orderCreated.OrderId);

        // Convert to integration event format
        var integrationEvent = new OrderCreatedIntegrationEvent(
            orderCreated.OrderId, 
            orderCreated.CustomerId, 
            orderCreated.TotalAmount);

        await _eventBus.PublishAsync(integrationEvent);
    }
}

// Audit logging for order creation
public class AuditLogConsumer : IConsumer<OrderCreatedEvent>
{
    private readonly ILogger<AuditLogConsumer> _logger;

    public AuditLogConsumer(ILogger<AuditLogConsumer> logger)
    {
        _logger = logger;
    }

    public Task Consume(ConsumeContext<OrderCreatedEvent> context)
    {
        var orderCreated = context.Message;

        _logger.LogInformation("AUDIT: Order {OrderId} placed by customer {CustomerId} " +
            "at {Time} with total amount {TotalAmount:C}", 
            orderCreated.OrderId, 
            orderCreated.CustomerId, 
            orderCreated.CreatedAt, 
            orderCreated.TotalAmount);

        return Task.CompletedTask;
    }
}