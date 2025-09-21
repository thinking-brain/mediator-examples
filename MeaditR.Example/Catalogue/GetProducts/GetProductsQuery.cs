using Mediator.Core.Dtos;
using MediatR;

namespace MeaditR.Example.Catalogue.GetProducts;

public record class GetProductsQuery(IEnumerable<Guid> ProductIds) : IRequest<List<Product>>;
