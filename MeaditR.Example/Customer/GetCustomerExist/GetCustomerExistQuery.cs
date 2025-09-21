using MediatR;

namespace MeaditR.Example.Customer.GetCustomerExist;

public record class GetCustomerExistQuery(Guid Id): IRequest<bool>;