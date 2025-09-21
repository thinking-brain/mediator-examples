

using Mediator.Core.Dtos;
using Mediator.Core.Entities;

namespace Mediator.Core.Repositories;

public class InMemoryOrderRepository : IOrderRepository
{
    private readonly List<Order> _orders = new();

    public Task AddAsync(Order order, CancellationToken cancellationToken = default)
    {
        _orders.Add(order);
        return Task.CompletedTask;
    }

    public Task<Order?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var order = _orders.FirstOrDefault(o => o.Id == id);
        return Task.FromResult(order);
    }
}

public class InMemoryCustomerRepository : ICustomerRepository
{
    private readonly List<Customer> _customers =
    [
        new Customer(Guid.Parse("11111111-1111-1111-1111-111111111111"), "John Doe", "john@example.com"),
        new Customer(Guid.Parse("22222222-2222-2222-2222-222222222222"), "Jane Smith", "jane@example.com")
    ];

    public Task<bool> ExistsAsync(Guid customerId, CancellationToken cancellationToken = default)
    {
        var exists = _customers.Any(c => c.Id == customerId);
        return Task.FromResult(exists);
    }

    public Task<Customer?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var customer = _customers.FirstOrDefault(c => c.Id == id);
        return Task.FromResult(customer);
    }
}

public class InMemoryProductCatalog : IProductCatalog
{
    private readonly List<Product> _products = new()
    {
        new Product(Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), "Laptop", 999.99m, 10),
        new Product(Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), "Mouse", 29.99m, 50),
        new Product(Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccc"), "Keyboard", 79.99m, 25)
    };

    public Task<List<Product>> GetProductsAsync(List<Guid> productIds, CancellationToken cancellationToken = default)
    {
        var products = _products.Where(p => productIds.Contains(p.Id)).ToList();
        return Task.FromResult(products);
    }

    public Task<Product?> GetProductAsync(Guid productId, CancellationToken cancellationToken = default)
    {
        var product = _products.FirstOrDefault(p => p.Id == productId);
        return Task.FromResult(product);
    }
}