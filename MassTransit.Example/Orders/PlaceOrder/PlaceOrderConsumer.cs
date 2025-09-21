using Mediator.Core.Dtos;
using Mediator.Core.Repositories;
using CoreEntities = Mediator.Core.Entities;

namespace MassTransit.Example.Orders.PlaceOrder;

public class PlaceOrderConsumer : IConsumer<PlaceOrderCommand>
{
    private readonly IOrderRepository _orderRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly IProductCatalog _productCatalog;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly ILogger<PlaceOrderConsumer> _logger;

    public PlaceOrderConsumer(
        IOrderRepository orderRepository,
        ICustomerRepository customerRepository,
        IProductCatalog productCatalog,
        IPublishEndpoint publishEndpoint,
        ILogger<PlaceOrderConsumer> logger)
    {
        _orderRepository = orderRepository;
        _customerRepository = customerRepository;
        _productCatalog = productCatalog;
        _publishEndpoint = publishEndpoint;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<PlaceOrderCommand> context)
    {
        var command = context.Message;
        
        try
        {
            _logger.LogInformation("Processing place order command for customer {CustomerId}", command.CustomerId);

            // 1. Validate business rules
            if (!await _customerRepository.ExistsAsync(command.CustomerId))
            {
                await context.RespondAsync(new PlaceOrderResult 
                { 
                    Success = false, 
                    ErrorMessage = "Invalid customer" 
                });
                return;
            }

            var productIds = command.Lines.Select(l => l.ProductId).ToList();
            var products = await _productCatalog.GetProductsAsync(productIds);
            
            if (products.Count != command.Lines.Count)
            {
                await context.RespondAsync(new PlaceOrderResult 
                { 
                    Success = false, 
                    ErrorMessage = "Some products do not exist" 
                });
                return;
            }

            // 2. Create and persist order
            var order = CreateOrder(command, products);
            await _orderRepository.AddAsync(order);

            // 3. Publish domain event
            var orderCreatedEvent = new OrderCreatedEvent
            {
                OrderId = order.Id,
                CustomerId = order.CustomerId,
                TotalAmount = order.TotalAmount,
                CreatedAt = order.CreatedAt,
                Lines = command.Lines
            };

            await _publishEndpoint.Publish(orderCreatedEvent);

            // 4. Respond with success
            await context.RespondAsync(new PlaceOrderResult 
            { 
                OrderId = order.Id,
                Success = true 
            });

            _logger.LogInformation("Successfully processed order {OrderId} for customer {CustomerId}", 
                order.Id, command.CustomerId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing place order command for customer {CustomerId}", command.CustomerId);
            
            await context.RespondAsync(new PlaceOrderResult 
            { 
                Success = false, 
                ErrorMessage = $"An error occurred: {ex.Message}" 
            });
        }
    }

    private static CoreEntities.Order CreateOrder(PlaceOrderCommand command, List<Product> products)
    {
        // Convert MassTransit OrderLineItems to Core OrderLineDtos
        var orderLineDtos = command.Lines.Select(line => 
            new OrderLineDto(line.ProductId, line.Quantity, line.UnitPrice)).ToList();

        // Use the static factory method from the Order entity
        return CoreEntities.Order.Create(command.CustomerId, orderLineDtos, products);
    }
}