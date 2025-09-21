namespace Mediator.Core.Services;

public interface IEmailService
{
    Task SendAsync(Guid customerId, string message, CancellationToken cancellationToken = default);
}

public interface ILoyaltyService
{
    Task AddPointsAsync(Guid customerId, int points, CancellationToken cancellationToken = default);
}

public interface IEventBus
{
    Task PublishAsync<T>(T eventMessage, CancellationToken cancellationToken = default) where T : class;
}