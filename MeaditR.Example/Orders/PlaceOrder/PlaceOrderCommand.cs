using MediatR;
using MeaditR.Example.Orders;
using Mediator.Core.Dtos;

namespace MeaditR.Example.Orders.PlaceOrder;

// Request to place an order
public record PlaceOrderCommand(Guid CustomerId, List<OrderLineDto> Lines) : IRequest<Guid>;

// Domain event raised when order is persisted
public record OrderCreated(Guid OrderId, Guid CustomerId, decimal TotalAmount) : INotification;
