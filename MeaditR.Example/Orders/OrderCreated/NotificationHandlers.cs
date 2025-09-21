using MediatR;
using Mediator.Core.Services;
using Mediator.Core.Dtos;

namespace MeaditR.Example.Orders.OrderCreated;

// Send confirmation email
public class SendConfirmationEmailHandler : INotificationHandler<PlaceOrder.OrderCreated>
{
    private readonly IEmailService _email;
    public SendConfirmationEmailHandler(IEmailService email) => _email = email;

    public Task Handle(PlaceOrder.OrderCreated evt, CancellationToken ct) =>
        _email.SendAsync(evt.CustomerId, $"Your order {evt.OrderId} was placed!");
}

// Add loyalty points
public class AddLoyaltyPointsHandler : INotificationHandler<PlaceOrder.OrderCreated>
{
    private readonly ILoyaltyService _loyalty;
    public AddLoyaltyPointsHandler(ILoyaltyService loyalty) => _loyalty = loyalty;

    public Task Handle(PlaceOrder.OrderCreated evt, CancellationToken ct) =>
        _loyalty.AddPointsAsync(evt.CustomerId, (int)(evt.TotalAmount / 10));
}

// Publish integration event to message bus
public class PublishIntegrationEventHandler : INotificationHandler<PlaceOrder.OrderCreated>
{
    private readonly IEventBus _bus;
    public PublishIntegrationEventHandler(IEventBus bus) => _bus = bus;

    public Task Handle(PlaceOrder.OrderCreated evt, CancellationToken ct) =>
        _bus.PublishAsync(new OrderCreatedIntegrationEvent(evt.OrderId, evt.CustomerId, evt.TotalAmount));
}

// Audit logging
public class AuditLogHandler : INotificationHandler<PlaceOrder.OrderCreated>
{
    private readonly ILogger<AuditLogHandler> _logger;
    public AuditLogHandler(ILogger<AuditLogHandler> logger) => _logger = logger;

    public Task Handle(PlaceOrder.OrderCreated evt, CancellationToken ct)
    {
        _logger.LogInformation("Order {OrderId} placed by customer {CustomerId} at {Time}", evt.OrderId, evt.CustomerId, DateTime.UtcNow);
        return Task.CompletedTask;
    }
}
