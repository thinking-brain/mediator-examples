using Mediator.Core.Repositories;
using MediatR;

namespace MeaditR.Example.Customer.GetCustomerExist;

public class GetCustomerExistHandler(ICustomerRepository customerRepository) : IRequestHandler<GetCustomerExistQuery, bool>
{
    public Task<bool> Handle(GetCustomerExistQuery request, CancellationToken cancellationToken)
    {
        return customerRepository.ExistsAsync(request.Id, cancellationToken);
    }
}
