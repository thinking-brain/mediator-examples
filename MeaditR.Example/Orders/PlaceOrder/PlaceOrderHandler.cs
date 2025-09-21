using MediatR;
using Mediator.Core.Entities;
using Mediator.Core.Repositories;
using MeaditR.Example.Catalogue.GetProducts;
using MeaditR.Example.Customer.GetCustomerExist;

namespace MeaditR.Example.Orders.PlaceOrder;

public class PlaceOrderHandler : IRequestHandler<PlaceOrderCommand, Guid>
{
    private readonly IOrderRepository _orders;
    private readonly IMediator _mediator;

    public PlaceOrderHandler(IOrderRepository orders, IMediator mediator)
    {
        _orders = orders;
        _mediator = mediator;
    }

    public async Task<Guid> Handle(PlaceOrderCommand request, CancellationToken ct)
    {
        // 1. Validate business rules
        if (!await _mediator.Send(new GetCustomerExistQuery(request.CustomerId), ct))
            throw new InvalidOperationException("Invalid customer");

        var products = await _mediator.Send(new GetProductsQuery(request.Lines.Select(l => l.ProductId)), ct);
        if (products.Count != request.Lines.Count)
            throw new InvalidOperationException("Some products do not exist");

        // 2. Persist order
        var order = Order.Create(request.CustomerId, request.Lines, products);
        await _orders.AddAsync(order, ct);

        // 3. Publish domain event
        await _mediator.Publish(new OrderCreated(order.Id, order.CustomerId, order.TotalAmount));

        return order.Id;
    }
}
