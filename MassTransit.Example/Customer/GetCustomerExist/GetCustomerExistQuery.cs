namespace MassTransit.Example.Customer.GetCustomerExist;

// Query to check if customer exists
public record GetCustomerExistQuery
{
    public Guid CustomerId { get; init; }
}

// Response for customer exist query
public record GetCustomerExistResponse
{
    public bool Exists { get; init; }
    public bool Success { get; init; }
    public string? ErrorMessage { get; init; }
}