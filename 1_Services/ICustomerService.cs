using DbModels;

namespace Services;

public interface ICustomerService
{
    public Task<List<CustomerDbM>> ReadAllCustomersAsync();
}


