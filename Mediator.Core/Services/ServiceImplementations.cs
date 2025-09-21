using Microsoft.Extensions.Logging;

namespace Mediator.Core.Services;

public class EmailService : IEmailService
{
    private readonly ILogger<EmailService> _logger;

    public EmailService(ILogger<EmailService> logger)
    {
        _logger = logger;
    }

    public Task SendAsync(Guid customerId, string message, CancellationToken cancellationToken = default)
    {
        // Simulate sending email
        _logger.LogInformation("Sending email to customer {CustomerId}: {Message}", customerId, message);
        return Task.CompletedTask;
    }
}

public class LoyaltyService : ILoyaltyService
{
    private readonly ILogger<LoyaltyService> _logger;

    public LoyaltyService(ILogger<LoyaltyService> logger)
    {
        _logger = logger;
    }

    public Task AddPointsAsync(Guid customerId, int points, CancellationToken cancellationToken = default)
    {
        // Simulate adding loyalty points
        _logger.LogInformation("Adding {Points} loyalty points to customer {CustomerId}", points, customerId);
        return Task.CompletedTask;
    }
}

public class EventBus : IEventBus
{
    private readonly ILogger<EventBus> _logger;

    public EventBus(ILogger<EventBus> logger)
    {
        _logger = logger;
    }

    public Task PublishAsync<T>(T eventMessage, CancellationToken cancellationToken = default) where T : class
    {
        // Simulate publishing to message bus
        _logger.LogInformation("Publishing integration event: {@Event}", eventMessage);
        return Task.CompletedTask;
    }
}