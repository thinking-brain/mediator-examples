using Mediator.Core.Repositories;

namespace MassTransit.Example.Catalogue.GetProducts;

// Consumer for product queries
public class GetProductsConsumer : IConsumer<GetProductsQuery>
{
    private readonly IProductCatalog _productCatalog;
    private readonly ILogger<GetProductsConsumer> _logger;

    public GetProductsConsumer(IProductCatalog productCatalog, ILogger<GetProductsConsumer> logger)
    {
        _productCatalog = productCatalog;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<GetProductsQuery> context)
    {
        var query = context.Message;
        
        try
        {
            _logger.LogInformation("Getting products for {Count} product IDs", query.ProductIds.Count);

            var products = await _productCatalog.GetProductsAsync(query.ProductIds);

            await context.RespondAsync(new GetProductsResponse
            {
                Products = products,
                Success = true
            });

            _logger.LogInformation("Found {Count} products", products.Count);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting products");
            
            await context.RespondAsync(new GetProductsResponse
            {
                Success = false,
                ErrorMessage = ex.Message
            });
        }
    }
}