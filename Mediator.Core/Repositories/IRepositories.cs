using Mediator.Core.Dtos;
using Mediator.Core.Entities;

namespace Mediator.Core.Repositories;

public interface IOrderRepository
{
    Task AddAsync(Order order, CancellationToken cancellationToken = default);
    Task<Order?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
}

public interface ICustomerRepository
{
    Task<bool> ExistsAsync(Guid customerId, CancellationToken cancellationToken = default);
    Task<Customer?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
}

public interface IProductCatalog
{
    Task<List<Product>> GetProductsAsync(List<Guid> productIds, CancellationToken cancellationToken = default);
    Task<Product?> GetProductAsync(Guid productId, CancellationToken cancellationToken = default);
}