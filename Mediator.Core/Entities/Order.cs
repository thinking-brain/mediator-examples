using Mediator.Core.Dtos;

namespace Mediator.Core.Entities;

public class Order
{
    public Guid Id { get; private set; }
    public Guid CustomerId { get; private set; }
    public List<OrderLine> Lines { get; private set; }
    public decimal TotalAmount { get; private set; }
    public DateTime CreatedAt { get; private set; }

    private Order() 
    {
        Lines = new List<OrderLine>();
    }

    public static Order Create(Guid customerId, List<OrderLineDto> lineItems, List<Product> products)
    {
        var order = new Order
        {
            Id = Guid.NewGuid(),
            CustomerId = customerId,
            CreatedAt = DateTime.UtcNow
        };

        var lines = new List<OrderLine>();
        foreach (var lineItem in lineItems)
        {
            var product = products.First(p => p.Id == lineItem.ProductId);
            lines.Add(new OrderLine(lineItem.ProductId, product.Name, lineItem.Quantity, product.Price));
        }

        order.Lines = lines;
        order.TotalAmount = lines.Sum(l => l.Quantity * l.UnitPrice);

        return order;
    }
}

public record OrderLine(Guid ProductId, string ProductName, int Quantity, decimal UnitPrice);