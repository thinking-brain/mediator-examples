using Mediator.Core.Repositories;

namespace MassTransit.Example.Customer.GetCustomerExist;

// Consumer for customer existence queries
public class GetCustomerExistConsumer : IConsumer<GetCustomerExistQuery>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly ILogger<GetCustomerExistConsumer> _logger;

    public GetCustomerExistConsumer(ICustomerRepository customerRepository, ILogger<GetCustomerExistConsumer> logger)
    {
        _customerRepository = customerRepository;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<GetCustomerExistQuery> context)
    {
        var query = context.Message;
        
        try
        {
            _logger.LogInformation("Checking if customer {CustomerId} exists", query.CustomerId);

            var exists = await _customerRepository.ExistsAsync(query.CustomerId);

            await context.RespondAsync(new GetCustomerExistResponse
            {
                Exists = exists,
                Success = true
            });

            _logger.LogInformation("Customer {CustomerId} exists: {Exists}", query.CustomerId, exists);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking customer existence for {CustomerId}", query.CustomerId);
            
            await context.RespondAsync(new GetCustomerExistResponse
            {
                Success = false,
                ErrorMessage = ex.Message
            });
        }
    }
}