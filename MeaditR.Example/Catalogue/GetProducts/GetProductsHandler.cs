using Mediator.Core.Dtos;
using Mediator.Core.Repositories;
using MediatR;

namespace MeaditR.Example.Catalogue.GetProducts;

public class GetProductsHandler(IProductCatalog productCatalog) : IRequestHandler<GetProductsQuery, List<Product>>
{
    public Task<List<Product>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        return productCatalog.GetProductsAsync([.. request.ProductIds], cancellationToken);
    }
}
