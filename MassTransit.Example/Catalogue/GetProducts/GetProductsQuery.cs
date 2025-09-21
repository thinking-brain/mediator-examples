using Mediator.Core.Dtos;

namespace MassTransit.Example.Catalogue.GetProducts;

// Query to get products by IDs
public record GetProductsQuery
{
    public List<Guid> ProductIds { get; init; } = new();
}

// Response for get products query
public record GetProductsResponse
{
    public List<Product> Products { get; init; } = new();
    public bool Success { get; init; }
    public string? ErrorMessage { get; init; }
}